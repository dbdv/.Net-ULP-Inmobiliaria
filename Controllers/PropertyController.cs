using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;

namespace testNetMVC.Controllers;

public class PropertyController : Controller
{
    private readonly ILogger<PropertyController> _logger;
    private readonly PropertyRepository propertyRepository;

    public PropertyController(ILogger<PropertyController> logger)
    {
        _logger = logger;
        propertyRepository = new PropertyRepository();
    }

    public IActionResult Index()
    {
        TypeRepository type_repo = new TypeRepository();
        PurposeRepository purpose_repo = new PurposeRepository();
        OwnerRepository owner_repo = new OwnerRepository();
        RenterRepository renter_repo = new RenterRepository();

        List<Property>? properties = propertyRepository.getAll();
        ViewBag.types = (List<PropType>)type_repo.getAll();
        ViewBag.purposes = (List<Purpose>)purpose_repo.getAll();
        ViewBag.owners = (List<Owner>)owner_repo.getAll();
        ViewBag.renters = (List<Renter>)renter_repo.getAll();

        if (properties is null) return View("loginError");

        return View(properties);
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

        return Redirect("/" + nameof(Property));
    }

    /*

    [HttpPut]
    // [ValidateAntiForgeryToken]
    [Route("Property/update")]
    public IActionResult Update([FromBody] Property body)
    {
        if (body.Email == string.Empty || body.First_name == string.Empty || body.Last_name == string.Empty || body.Dni == string.Empty)
            return BadRequest("Datos incorrectos.");

        int updated = propertyRepository.update(body);

        if (updated == -1) return Problem("No se pudo actualizar la información del inquilino");

        return Redirect("/Property");
    }
    */

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
