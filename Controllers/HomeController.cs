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
    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Login([FromBody] UserBody userBody)
    {
        if (userBody.Email == string.Empty || userBody.Password == string.Empty)
            return BadRequest();

        //---------------------------------------------
        var dbUser = userRepo.getByEmail(userBody.Email);

        if (dbUser == null)
        {
            return NotFound();
        }

        string hashed = Models.User.getHashPassword(userBody.Password);

        if (dbUser.Password != hashed)
        {
            return NotFound();
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, dbUser.Email),
            new Claim(ClaimTypes.Role, dbUser.Role.Label),
            new Claim(type: "UserId", value: dbUser.Id.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        return Redirect("/home");
    }

    [HttpGet]
    [Route("/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
    [Authorize]
    [Route("/home")]
    public IActionResult Home()
    {
        return View("Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
