using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;

namespace testNetMVC.Controllers;

public class HomeController : Controller
{
    public class UserBody
    {
        public string email { get; set; }
        public string password { get; set; }
    }
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
        TypeRepository type_repo = new TypeRepository();

        PurposeRepository purpose_repo = new PurposeRepository();

        // ViewBag.purposes = (List<Purpose>)purpose_repo.getAll();

        // if (user == null) return View("loginError"); // Control para la parte de auth
        if (userLogged == null) return View(new User { Name = "Falso Usuario" });

        return View(userLogged);
    }
    [Route("/home")]
    public IActionResult Home()
    {
        return View("Home");
    }
    [HttpPost]
    [Route("/login")]
    public IActionResult Login([FromBody] UserBody userBody)
    {
        if (userBody.email == string.Empty || userBody.password == string.Empty)
            return BadRequest();
        return Json((dynamic)new
        {
            user_name = userBody.email
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
