using LendingApi.Application.Services;
using LendingApi.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LendingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService service, ILogger<CustomersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _service.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _service.GetCustomerById(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var newCustomer = await _service.CreateCustomer(customer);
            _logger.LogInformation("Customer added: {Customer}", customer.FirstName + ' ' + customer.LastName);

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var customerUpdated = await _service.UpdateCustomer(id, customer);
            if (!customerUpdated)
            {
                _logger.LogWarning("Attempted to update non-existent customer with id: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Customer updated: {Customer}", customer.FirstName + ' ' + customer.LastName);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _service.DeleteCustomer(id);
            if (!customer)
            {
                _logger.LogWarning("Attempted to delete non-existent customer with id: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Customer deleted: {Id}", id);
            return NoContent();
        }
    }
}
