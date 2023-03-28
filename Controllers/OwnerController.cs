using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;

namespace testNetMVC.Controllers;

public class OwnerController : Controller
{
    private readonly ILogger<OwnerController> _logger;
    private readonly OwnerRepository ownerRepository;

    public OwnerController(ILogger<OwnerController> logger)
    {
        _logger = logger;
        ownerRepository = new OwnerRepository();
    }

    public IActionResult Index()
    {

        List<Owner>? owners = ownerRepository.getAll();

        if (owners is null) return View("loginError");

        return View(owners);
    }

    [HttpGet]
    // [ValidateAntiForgeryToken]
    public IActionResult All()
    {

        List<Owner>? owners = ownerRepository.getAll();

        if (owners is null) return Problem("No se pudo recuperar los propietarios.", statusCode: 501);
        return Json(owners);
    }

    [HttpGet]
    // [ValidateAntiForgeryToken]
    public IResult One()
    {

        List<Owner>? owners = ownerRepository.getAll();

        return Results.Ok(owners);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route(nameof(Owner) + "/new")]
    public IActionResult New([FromBody] Owner body)
    {

        if (body.Email == string.Empty || body.First_name == string.Empty || body.Last_name == string.Empty || body.Dni == string.Empty)
            return BadRequest("Datos incorrectos.");

        int created = ownerRepository.create(body);
        if (created == -1) return Problem("No se pudo crear el propietario", statusCode: 501);

        return Redirect("/" + nameof(Owner));
    }

    [HttpPut]
    // [ValidateAntiForgeryToken]
    [Route(nameof(Owner) + "/update")]
    public IActionResult Update([FromBody] Owner body)
    {
        if (body.Email == string.Empty || body.First_name == string.Empty || body.Last_name == string.Empty || body.Dni == string.Empty)
            return BadRequest("Datos incorrectos.");

        int updated = ownerRepository.update(body);

        if (updated == -1) return Problem("No se pudo actualizar la información del propietario");

        return Redirect("/" + nameof(Owner));
    }

    [HttpGet]
    [Route(nameof(Owner) + "/get")]
    // [ValidateAntiForgeryToken]
    public IActionResult Get([FromQuery] int id)
    {
        if (id < 1)
            return Problem("ID inválido.", statusCode: 400);
        var owner = ownerRepository.get(id);

        if (owner is null) return Problem("No se pudo acceder a los datos del usuario", statusCode: 500);

        // return Redirect("/Owner");
        return Json(owner);
    }

    [HttpDelete]
    [Route(nameof(Owner) + "/delete")]
    // [ValidateAntiForgeryToken]
    public IActionResult Delete([FromQuery] int id)
    {
        if (id < 1)
            return BadRequest("ID inválido.");
        var result = ownerRepository.delete(id);

        if (result == -1) return Problem("No se pudo borrar al propietario.", statusCode: 500);

        return Redirect("/" + nameof(Owner));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
