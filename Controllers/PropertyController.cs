using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace testNetMVC.Controllers;

[Authorize]
public class PropertyController : Controller
{
    private readonly ILogger<PropertyController> _logger;
    private readonly PropertyRepository propertyRepository;
    private readonly IDictionary<string, string> accionsTempMsgs = new Dictionary<string, string>{
        {"created","Propiedad agregada correctamente."},
    };

    public PropertyController(ILogger<PropertyController> logger)
    {
        _logger = logger;
        propertyRepository = new PropertyRepository();
    }

    public IActionResult Index()
    {
        OwnerRepository owner_repo = new OwnerRepository();
        RenterRepository renter_repo = new RenterRepository();

        List<Property>? properties = propertyRepository.getAll();
        ViewBag.types = propertyRepository.getPropTypes();
        ViewBag.purposes = propertyRepository.getPurposes();
        ViewBag.owners = owner_repo.getAll();
        ViewBag.renters = renter_repo.getAll();

        if (properties is null) return View("loginError");

        return View(properties);
    }
    
    [HttpGet]
    [Route("Property/ByOwner/{id}")]
    public IActionResult getByOwner(int id)
    {
        OwnerRepository owner_repo = new OwnerRepository();
        RenterRepository renter_repo = new RenterRepository();

        List<Property>? properties = propertyRepository.getAllByOwner(id);
        ViewBag.types = propertyRepository.getPropTypes();
        ViewBag.purposes = propertyRepository.getPurposes();
        ViewBag.owners = owner_repo.getAll();
        ViewBag.owner = owner_repo.get(id);

        if (properties is null) return View("loginError");

        return View("Property", properties);
    }

    [HttpGet]
    // [ValidateAntiForgeryToken]
    public IActionResult All()
    {

        List<Property>? properties = propertyRepository.getAll();

        if (properties is null) return Problem("No se pudo recuperar los inquilinos.", statusCode: 501);
        return Json(properties);
    }

    [HttpGet]
    // [ValidateAntiForgeryToken]
    public IResult One()
    {

        List<Property>? properties = propertyRepository.getAll();

        return Results.Ok(properties);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Property")]
    public IActionResult Create([FromBody] Property body)
    {

        if (body.Address == string.Empty || body.Owner_id < 1 || body.Price < 1 || body.Purpose_id < 1 || body.Rooms < 1 || body.Type_id < 1)
            return BadRequest("Datos incorrectos.");

        int created = propertyRepository.create(body);
        if (created == -1) return Problem("No se pudo agregar la propiedad", statusCode: 501);
        TempData["ProcessMsg"] = accionsTempMsgs["created"];
        return Redirect("/" + nameof(Property));
    }

    [HttpGet]
    [Route(nameof(Property) + "/unavailableDates/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult GetUnavailableDates(int id)
    {
        if (id < 1)
            return Problem("ID inválido.", statusCode: 400);
        var dates = propertyRepository.getUnavailableDates(id);

        if (dates is null) return Problem("No se pudo acceder a los datos del usuario", statusCode: 500);

        return Json(dates);
    }

    [HttpGet]
    [Route("Property/{id}")]
    // [ValidateAntiForgeryToken]
    public IActionResult Get(int id)
    {
        if (id < 1)
            return Problem("ID inválido.", statusCode: 400);
        var owner = propertyRepository.get(id);

        if (owner is null) return Problem("No se pudo acceder a los datos del inquilino", statusCode: 500);

        // return Redirect("/Property");
        return Json(owner);
    }

    [HttpDelete]
    [Authorize(Policy = "admin")]
    [Route("Property/{id}")]
    // [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        if (id < 1)
            return BadRequest("ID inválido.");
        var result = propertyRepository.delete(id);

        if (result == -1) return Problem("No se pudo borrar el inmueble.", statusCode: 500);

        return Redirect("/" + nameof(Property));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
