using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace testNetMVC.Controllers;
public class UserController : Controller
{
    private readonly IDictionary<string, string> accionsTempMsgs = new Dictionary<string, string>{
        {"created","Usuario creado correctamente."},
    };
    private readonly ILogger<UserController> _logger;
    private UserRepository userRepo = new UserRepository();

    public UserController(ILogger<UserController> logger, ILoggerFactory factory)
    {
        _logger = logger;
    }

    [Authorize(Policy = "admin")]
    public IActionResult Index()
    {
        ViewBag.users = (List<User>)userRepo.getAll();
        ViewBag.roles = (List<Role>)userRepo.getRoles();

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Policy = "admin")]
    [Route("users/")]
    public IActionResult getAll()
    {
        var users = userRepo.getAll();
        return Json(users);
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    [Route("users/")]
    public IActionResult createUser([FromBody] User user)
    {
        if (user.Email == string.Empty || user.Password == string.Empty || !(user.Role_id >= 0))
            return BadRequest();

        var hashedPass = Models.User.getHashPassword(user.Password);

        user.Password = hashedPass;

        int created = userRepo.create(user);

        if (created == -1)
            return Error();

        TempData["ProcessMsg"] = accionsTempMsgs["created"];
        return Redirect("/admin");
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    [Route("users/{id}")]
    public IActionResult deleteUser(int id)
    {
        if (!(id >= 0))
            return BadRequest();

        int deleted = userRepo.delete(id);

        if (deleted == -1)
            return Error();

        return Redirect("/admin");
    }

    [Route("Profile")]
    public IActionResult Profile()
    {
        int userId = Convert.ToInt32(User.FindFirstValue("UserId"));
        var user = userRepo.get(userId);
        // defaultProfilePath = System.AppDomain.CurrentDomain.BaseDirectory + defaultProfilePath.Replace('/', '\\');
        // ViewBag.defaultProfilePath = defaultProfilePath;

        return View("Profile", user);
    }
}