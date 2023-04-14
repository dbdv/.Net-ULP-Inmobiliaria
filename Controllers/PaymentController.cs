using Microsoft.AspNetCore.Mvc;
using testNetMVC.Models;
using testNetMVC.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace testNetMVC.Controllers;

[Authorize]
public class PaymentController : Controller
{
    private readonly ILogger<PaymentController> _logger;
    private readonly ContractRepository contractRepository;

    public PaymentController(ILogger<PaymentController> logger)
    {
        _logger = logger;
        contractRepository = new ContractRepository();
    }

    public IActionResult Index()
    {

        List<Payment>? payments = contractRepository.getAllPayments();

        if (payments is null) return View("loginError");

        ViewBag.renters = new RenterRepository().getAll();
        ViewBag.properties = new PropertyRepository().getAll();
        ViewBag.contracts = contractRepository.getAll();

        return View(payments);
    }

    [HttpGet]
    // [ValidateAntiForgeryToken]
    public IActionResult All()
    {

        List<Payment>? payments = contractRepository.getAllPayments();

        if (payments is null) return Problem("No se pudo recuperar los pagos.", statusCode: 501);
        return Json(payments);
    }

    [HttpPost]
    // [ValidateAntiForgeryToken]
    [Route(nameof(Payment))]
    public IActionResult New([FromBody] BodyPayment? body)
    {
        if (body == null
            || body.Amount == null
            || body.Contract_id == null
            || body.Date == null
            || !ModelState.IsValid
        )
            return BadRequest("Datos incorrectos.");

        string parsedDate = String.Join("/", body.Date.Split('/').Reverse());


        var payment = new Payment
        {
            Amount = body.Amount,
            Contract_id = body.Contract_id,
            Date = Convert.ToDateTime(parsedDate)
        };

        int created = contractRepository.createPayment(payment);
        if (created == -1) return Problem("No se pudo crear el pago", statusCode: 501);

        return Redirect("/" + nameof(Payment));
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
    [Route(nameof(Payment) + "/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Get(int id)
    {
        if (id < 1)
            return Problem("ID inválido.", statusCode: 400);
        var payment = contractRepository.getPayment(id);

        if (payment is null) return Problem("No se pudo acceder a los datos del usuario", statusCode: 500);

        // return Redirect("/Contract");
        return Json(payment);
    }

    [HttpDelete]
    [Authorize(Policy = "admin")]
    [Route(nameof(Payment) + "/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        if (id < 1)
            return BadRequest("ID inválido.");
        var result = contractRepository.deletePayment(id);

        if (result == -1) return Problem("No se pudo borrar el pago.", statusCode: 500);

        return Redirect("/" + nameof(Payment));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }


    public class BodyPayment
    {
        public Decimal? Amount { get; set; }
        public string? Date { get; set; }
        public int? Contract_id { get; set; }
    }
}
