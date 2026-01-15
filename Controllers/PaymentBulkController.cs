using LendingApi.Application.Services.CsvImport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LendingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentBulkController : ControllerBase
    {
        private readonly IPaymentBulkImportService _bulkImportService;
        private readonly ILogger<PaymentBulkController> _logger;

        public PaymentBulkController(
            IPaymentBulkImportService bulkImportService,
            ILogger<PaymentBulkController> logger)
        {
            _bulkImportService = bulkImportService;
            _logger = logger;
        }

        [HttpPost("import")]
        [RequestSizeLimit(52428800)]
        public async Task<ActionResult<BulkImportResult>> ImportPayments(
            IFormFile file,
            CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded" });

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { error = "Only CSV files are allowed" });

            try
            {
                using var stream = file.OpenReadStream();
                var result = await _bulkImportService.ImportPaymentsFromCsvAsync(stream, cancellationToken);

                if (result.Errors.Any())
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing CSV import");
                return StatusCode(500, new { error = "An error occurred while processing the file"});
            }
        }
    }
}
