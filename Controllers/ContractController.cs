using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace testNetMVC.Controllers;

[Authorize]
public class ContractController : Controller
{
    private readonly ILogger<ContractController> _logger;
    private readonly ContractRepository contractRepository;

    public ContractController(ILogger<ContractController> logger)
    {
        _logger = logger;
        contractRepository = new ContractRepository();
    }

    public IActionResult Index()
    {

        List<Contract>? contracts = contractRepository.getAll();

        if (contracts is null) return View("loginError");

        ViewBag.renters = new RenterRepository().getAll();
        ViewBag.properties = new PropertyRepository().getAll();

        return View(contracts);
    }

    [HttpGet]
    // [ValidateAntiForgeryToken]
    public IActionResult All()
    {

        List<Contract>? contracts = contractRepository.getAll();

        if (contracts is null) return Problem("No se pudo recuperar los contratos.", statusCode: 501);
        return Json(contracts);
    }

    [HttpGet]
    // [ValidateAntiForgeryToken]
    public IResult One()
    {

        List<Contract>? contracts = contractRepository.getAll();

        return Results.Ok(contracts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route(nameof(Contract))]
    public IActionResult New([FromBody] Contract body)
    {

        if (body.Fee == null
            || body.Until == null
            || body.From == null
            || body.Property_id == null
            || body.Renter_id == null
        )
            return BadRequest("Datos incorrectos.");

        int created = contractRepository.create(body);
        if (created == -1) return Problem("No se pudo crear el contrato", statusCode: 501);

        return Redirect("/" + nameof(Contract));
    }

    /* [HttpPut]
    [ValidateAntiForgeryToken]
    [Route(nameof(Contract) + "/update")]
    public IActionResult Update([FromBody] Contract body)
    {
        if (body.Fee == null
            || body.Until == null
            || body.From == null
            || body.Property_id == null
            || body.Renter_id == null
        )
            return BadRequest("Datos incorrectos.");

        int updated = contractRepository.update(body);

        if (updated == -1) return Problem("No se pudo actualizar la información del contrato");

        return Redirect("/" + nameof(Contract));
    } */

    [HttpGet]
    [Route(nameof(Contract) + "/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Get(int id)
    {
        if (id < 1)
            return Problem("ID inválido.", statusCode: 400);
        var contract = contractRepository.get(id);

        if (contract is null) return Problem("No se pudo acceder a los datos del usuario", statusCode: 500);

        // return Redirect("/Contract");
        return Json(contract);
    }


    [HttpDelete]
    [Authorize(Policy = "admin")]
    [Route(nameof(Contract) + "/delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete([FromQuery] int id)
    {
        if (id < 1)
            return BadRequest("ID inválido.");
        var result = contractRepository.delete(id);

        if (result == -1) return Problem("No se pudo borrar al contrato.", statusCode: 500);

        return Redirect("/" + nameof(Contract));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
