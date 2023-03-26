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
        // ownerRepository = new OwnerRepository(logger);
        ownerRepository = new OwnerRepository();
    }

    public IActionResult Index()
    {

        List<Owner> owners = ownerRepository.getAll();

        // if (owner == null) return View("loginError");
        Console.WriteLine(owners.Count());

        return View(owners);
    }

    [HttpGet]
    [ValidateAntiForgeryToken]
    public IResult All()
    {

        List<Owner> owners = ownerRepository.getAll();

        return Results.Ok(owners);
    }

    [HttpGet]
    [ValidateAntiForgeryToken]
    public IResult One()
    {

        List<Owner> owners = ownerRepository.getAll();

        return Results.Ok(owners);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IResult New([FromBody] Owner body)
    {
        Console.WriteLine(body);
        Boolean created = Convert.ToBoolean(ownerRepository.create(body));


        if (!created) Results.Problem("No se pudo crear el usuario", statusCode: 500);

        return Results.Ok("Creado correctamente");
    }

    [HttpGet]
    [Route("Owner/get")]
    [ValidateAntiForgeryToken]
    public IResult New([FromQuery] int id)
    {
        Console.WriteLine(id);
        var owner = ownerRepository.get(id);


        if (owner == null) Results.Problem("No se pudo acceder a los datos del usuario", statusCode: 500);

        return Results.Ok(owner);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
