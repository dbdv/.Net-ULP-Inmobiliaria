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
    public IResult All()
    {
        Console.WriteLine("aca");

        List<Owner>? owners = ownerRepository.getAll();

        if (owners is null) return Results.Problem("No se pudo recuperar los propietarios.", statusCode: 501);
        return Results.Json(owners, statusCode: 200);
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
    public IResult New([FromBody] Owner body)
    {

        if (body.Email == string.Empty || body.First_name == string.Empty || body.Last_name == string.Empty || body.Dni == string.Empty)
            return Results.Problem("Datos incorrectos.", statusCode: 400);

        int created = ownerRepository.create(body);

        if (created == -1) return Results.Problem("No se pudo crear el propietario", statusCode: 501);

        return Results.Ok("Agregado correctamente");
    }

    [HttpPost]
    // [ValidateAntiForgeryToken]
    public IResult Update([FromBody] Owner body)
    {

        if (body.Email == string.Empty || body.First_name == string.Empty || body.Last_name == string.Empty || body.Dni == string.Empty)
            return Results.Problem("Datos incorrectos.", statusCode: 400);

        int updated = ownerRepository.create(body);

        if (updated == -1) return Results.Problem("No se pudo actualizar la información del propietario", statusCode: 501);

        return Results.Ok("Datos actualizados correctamente");
    }

    [HttpGet]
    [Route("Owner/get")]
    // [ValidateAntiForgeryToken]
    public IResult New([FromQuery] int id)
    {
        if (id < 1)
            return Results.Problem("ID inválido.", statusCode: 400);
        var owner = ownerRepository.get(id);

        if (owner is null) return Results.Problem("No se pudo acceder a los datos del usuario", statusCode: 500);

        return Results.Json(owner, statusCode: 200);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
