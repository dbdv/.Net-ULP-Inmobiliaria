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

        List<Property>? properties = propertyRepository.getAll();

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

    /*

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Property/new")]
    public IActionResult New([FromBody] Property body)
    {

        if (body.Email == string.Empty || body.First_name == string.Empty || body.Last_name == string.Empty || body.Dni == string.Empty)
            return BadRequest("Datos incorrectos.");

        int created = propertyRepository.create(body);
        if (created == -1) return Problem("No se pudo crear el inquilino", statusCode: 501);

        return Redirect("/Property");
    }

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
    [Route("Property/get")]
    // [ValidateAntiForgeryToken]
    public IActionResult Get([FromQuery] int id)
    {
        if (id < 1)
            return Problem("ID inválido.", statusCode: 400);
        var owner = propertyRepository.get(id);

        if (owner is null) return Problem("No se pudo acceder a los datos del inquilino", statusCode: 500);

        // return Redirect("/Property");
        return Json(owner);
    }

    /*
    [HttpDelete]
    [Route("Property/delete")]
    // [ValidateAntiForgeryToken]
    public IActionResult Delete([FromQuery] int id)
    {
        if (id < 1)
            return BadRequest("ID inválido.");
        var result = propertyRepository.delete(id);

        if (result == -1) return Problem("No se pudo borrar al inquilino.", statusCode: 500);

        return Redirect("/Property");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
    */
}
