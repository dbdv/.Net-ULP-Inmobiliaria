using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;

namespace testNetMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly OwnerRepository ownerRepository;

    public HomeController(ILogger<HomeController> logger, ILoggerFactory factory)
    {
        _logger = logger;
        // ownerRepository = new OwnerRepository((ILogger<OwnerRepository>)logger);
        ownerRepository = new OwnerRepository();
    }

    public IActionResult Index()
    {

        User? userLogged = null;

        // if (user == null) return View("loginError"); // Control para la parte de auth
        if (userLogged == null) return View(new User { Name = "Falso Usuario" });

        return View(userLogged);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
