using LendingApi.Application.Services;
using LendingApi.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LendingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentService service, ILogger<PaymentsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var payments = await _service.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _service.GetPaymentById(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment(Payment payment)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var newPayment = await _service.CreatePayment(payment);
            _logger.LogInformation("Payment added: {Payment}", payment.Id);

            return CreatedAtAction(nameof(GetPayment), new {id = payment.Id }, payment);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var paymentDeleted = await _service.DeletePayment(id);
            if (!paymentDeleted)
            {
                _logger.LogWarning("Attempted to delete non-existent payment with id: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Payment deleted: {Id}", id);
            return NoContent();
        }
    }
}
