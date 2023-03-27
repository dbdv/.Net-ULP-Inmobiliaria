using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;

namespace testNetMVC.Controllers;

public class RenterController : Controller
{
    private readonly ILogger<RenterController> _logger;
    private readonly RenterRepository renterRepository;

    public RenterController(ILogger<RenterController> logger)
    {
        _logger = logger;
        renterRepository = new RenterRepository();
    }

    public IActionResult Index()
    {

        List<Renter>? renters = renterRepository.getAll();

        if (renters is null) return View("loginError");

        return View(renters);
    }

    [HttpGet]
    // [ValidateAntiForgeryToken]
    public IActionResult All()
    {

        List<Renter>? renters = renterRepository.getAll();

        if (renters is null) return Problem("No se pudo recuperar los inquilinos.", statusCode: 501);
        return Json(renters);
    }

    [HttpGet]
    // [ValidateAntiForgeryToken]
    public IResult One()
    {

        List<Renter>? renters = renterRepository.getAll();

        return Results.Ok(renters);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Renter/new")]
    public IActionResult New([FromBody] Renter body)
    {

        if (body.Email == string.Empty || body.First_name == string.Empty || body.Last_name == string.Empty || body.Dni == string.Empty)
            return BadRequest("Datos incorrectos.");

        int created = renterRepository.create(body);
        if (created == -1) return Problem("No se pudo crear el inquilino", statusCode: 501);

        return Redirect("/Renter");
    }

    [HttpPut]
    // [ValidateAntiForgeryToken]
    [Route("Renter/update")]
    public IActionResult Update([FromBody] Renter body)
    {
        if (body.Email == string.Empty || body.First_name == string.Empty || body.Last_name == string.Empty || body.Dni == string.Empty)
            return BadRequest("Datos incorrectos.");

        int updated = renterRepository.update(body);

        if (updated == -1) return Problem("No se pudo actualizar la información del inquilino");

        return Redirect("/Renter");
    }

    [HttpGet]
    [Route("Renter/get")]
    // [ValidateAntiForgeryToken]
    public IActionResult Get([FromQuery] int id)
    {
        if (id < 1)
            return Problem("ID inválido.", statusCode: 400);
        var owner = renterRepository.get(id);

        if (owner is null) return Problem("No se pudo acceder a los datos del inquilino", statusCode: 500);

        // return Redirect("/Renter");
        return Json(owner);
    }

    [HttpDelete]
    [Route("Renter/delete")]
    // [ValidateAntiForgeryToken]
    public IActionResult Delete([FromQuery] int id)
    {
        if (id < 1)
            return BadRequest("ID inválido.");
        var result = renterRepository.delete(id);

        if (result == -1) return Problem("No se pudo borrar al inquilino.", statusCode: 500);

        return Redirect("/Renter");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
