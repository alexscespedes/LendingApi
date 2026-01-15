using LendingApi.Application.Services.CsvExport;
using LendingApi.Application.Services.DTOs;
using LendingApi.Data.SqlDatabase.Export;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LendingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentExportController : ControllerBase
    {
        private readonly IPaymentBulkExportService _bulkExportService;
        private readonly ILogger<PaymentExportController> _logger;

        public PaymentExportController(
            IPaymentBulkExportService bulkExportService,
            ILogger<PaymentExportController> logger)
        {
            _bulkExportService = bulkExportService;
            _logger = logger;
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportsPayments(
            [FromBody] PaymentExportFilter filter,
            CancellationToken cancellationToken)
        {
            try
            {
                var memoryStream = new MemoryStream();

                var result = await _bulkExportService.ExportPaymentsToCsvAsync(
                    memoryStream,
                    filter,
                    cancellationToken);

                if (!result.Success)
                    return BadRequest(new { error = result.ErrorMessage });

                memoryStream.Position = 0;

                var fileName = $"payments_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";

                return File(
                    memoryStream,
                    "text/csv",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing export request");
                return StatusCode(500, new { error = "An error occurred while exporting"});
            }
        }

        [HttpPost("export/streaming")]
        public async Task<IActionResult> ExportPaymentsStreaming(
            [FromBody] PaymentExportFilter filter,
            CancellationToken cancellationToken,
            [FromQuery] int batchSize = 1000)
        {
            try
            {
                var stream = new MemoryStream();

                var result = await _bulkExportService.ExportPaymentsToCsvStreamingAsync(
                    stream,
                    filter,
                    batchSize,
                    cancellationToken);

                if (!result.Success)
                    return BadRequest(new { error = result.ErrorMessage });

                stream.Position = 0;

                var fileName = $"payments_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";

                return File(
                    stream,
                    "text/csv",
                    fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing streaming export request");
                return StatusCode(500, new { error = "An error occurred while exporting payments" });
            }
        }

        [HttpPost("export/count")]
        public async Task<ActionResult<int>> GetExportCount(
            [FromBody] PaymentExportFilter filter,
            CancellationToken cancellationToken)
        {
            try
            {
                var repository = HttpContext.RequestServices.GetRequiredService<IPaymentExportRepository>();
                var count = await repository.GetPaymentsCountAsync(filter, cancellationToken);

                return Ok(new { count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting export count");
                return StatusCode(500, new { error = "An error occurred while counting records" });
            }
        }
    }
}
