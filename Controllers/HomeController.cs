using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace testNetMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private UserRepository userRepo = new UserRepository();

    public HomeController(ILogger<HomeController> logger, ILoggerFactory factory)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {

        if (User.Identity.IsAuthenticated)
            return Redirect("/home");

        // if (user == null) return View("loginError"); // Control para la parte de auth

        return View();
    }
    [Authorize]
    [Route("/home")]
    public IActionResult Home()
    {
        return View("Home");
    }
    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Login([FromBody] UserBody userBody)
    {
        if (userBody.email == string.Empty || userBody.password == string.Empty)
            return BadRequest();

        //---------------------------------------------
        var dbUser = userRepo.getByEmail(userBody.email);

        if (dbUser == null)
        {
            return NotFound();
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: userBody.password,
                        salt: System.Text.Encoding.ASCII.GetBytes("secretito"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
        if (dbUser.Password != hashed)
        {
            return NotFound();
        }
        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, dbUser.Email),
                                new Claim(ClaimTypes.Role, dbUser.Role.Label),
                            };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        return Redirect("/home");
    }

    [HttpPost]
    [Route("/logout")]
    public async Task<IActionResult> Logou()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
