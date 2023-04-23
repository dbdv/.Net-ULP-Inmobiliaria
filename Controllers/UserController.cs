using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Web;
using System.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace testNetMVC.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IDictionary<string, string> accionsTempMsgs = new Dictionary<string, string>{
        {"created","Usuario creado correctamente."},
        {"updated","Usuario actualizado correctamente."},
    };
    private readonly ILogger<UserController> _logger;
    private UserRepository userRepo = new UserRepository();

    public UserController(ILogger<UserController> logger, ILoggerFactory factory)
    {
        _logger = logger;
    }

    [Authorize(Policy = "admin")]
    [Route("users-managment")]
    public IActionResult Index()
    {
        var users = userRepo.getAll();
        ViewBag.users = users != null ? users : new List<User> { };
        ViewBag.roles = userRepo.getRoles();

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
    
    [HttpGet]
    [Authorize(Policy = "admin")]
    [Route("users/{id}")]
    public IActionResult getOne(int id)
    {
        var user = userRepo.get(id);
        user.Password = null;
        return Json(user);
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
        return Redirect("/users-managment");
    }

    [HttpPost]
    [Authorize]
    [Route("users/update")]
    public IActionResult updateProfile(UserBody user)
    {
        int userId = Convert.ToInt32(User.FindFirstValue("UserId"));
        user.Id = userId;
        var currentUserData = userRepo.get(userId);

        if (user.CurrentPassword != null
            && user.NewPassword != null
            )
        {
            var hashedPass = Models.User.getHashPassword(user.CurrentPassword);
            var passToMatch = currentUserData.Password;
            if (hashedPass != passToMatch)
                return BadRequest("PAsa por aca");
        }

        if (user.NewPassword == null)
            user.NewPassword = currentUserData.Password;

        if (user.Avatar != null)
        {
            string path = System.IO.Path.Combine("", "./wwwroot/uploads/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = "avatar_" + userId + Path.GetExtension(user.Avatar.FileName);
            string fullPath = Path.Combine(path, fileName);
            user.AvatarUrl = fileName;
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                user.Avatar.CopyTo(stream);
            }
        }

        if (user.AvatarUrl == null)
        {
            string fullPath = Path.Combine("./wwwroot/uploads/", currentUserData.Avatar);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            user.Avatar = null;
            user.AvatarUrl = null;
        }

        int updated = userRepo.updateProfile(user);
        if (updated == -1)
            return Error();

        TempData["ProcessMsg"] = accionsTempMsgs["created"];
        return Redirect("/profile");
    }

    [HttpPut]
    [Authorize(Policy = "admin")]
    [Route("users/{id}")]
    public IActionResult updateUser([FromBody]UserBody user, int id)
    {
        user.Id = id;
        var currentUserData = userRepo.get(id);

        if (user.Password == null || user.Password == String.Empty)
            user.Password = currentUserData.Password;

        int updated = userRepo.update(user);
        if (updated == -1)
            return Error();

        TempData["ProcessMsg"] = accionsTempMsgs["updated"];
        return Redirect("/users-managment");
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    [Route("users/{id}")]
    public IActionResult deleteUser(int id)
    {
        if (!(id >= 0))
            return BadRequest();

        string avatarPath = userRepo.get(id).Avatar;
        int deleted = userRepo.delete(id);

        if (deleted == -1)
            return Error();

        string fullPath = Path.Combine("./wwwroot/uploads/", avatarPath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }

        return Redirect("/users-managment");
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