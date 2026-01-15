using System;
using LendingApi.Application.Helpers;
using LendingApi.Application.Services.DTOs;
using LendingApi.Data.SqlDatabase.Export;

namespace LendingApi.Application.Services.CsvExport;

public class PaymentBulkExportService : IPaymentBulkExportService
{
    private readonly IPaymentExportRepository _exportRepository;
    private readonly ICsvGeneratorService _csvGenerator;
    private readonly ILogger<PaymentBulkExportService> _logger;
    private readonly CsvExportHelper _helper;

    public PaymentBulkExportService(
        IPaymentExportRepository exportRepository,
        ICsvGeneratorService csvGenerator,
        ILogger<PaymentBulkExportService> logger,
        CsvExportHelper helper
        )
    {
        _exportRepository = exportRepository;
        _csvGenerator = csvGenerator;
        _logger = logger;
        _helper = helper;
    }

    public async Task<BulkExportResult> ExportPaymentsToCsvAsync(Stream outputStream, PaymentExportFilter filter, CancellationToken cancellationToken = default)
    {
        var result = new BulkExportResult();

        try
        {
            var totalCount = await _exportRepository.GetPaymentsCountAsync(filter, cancellationToken);

            if (totalCount == 0)
            {
                result.Success = false;
                result.ErrorMessage = "No payments found matching the filter criterias";
                return result;
            }

            var payments = await _exportRepository.GetPaymentsForExportAsync(filter, cancellationToken);

            var startPosition = outputStream.Position;
            await _csvGenerator.WritePaymentCsvAsync(outputStream, payments, cancellationToken);
            var endPosition = outputStream.Position;

            result.Success = true;
            result.TotalRecords = totalCount;
            result.FileSizeBytes = endPosition - startPosition;

            _logger.LogInformation($"Successfully exported {result.TotalRecords} payments ({result.FileSizeBytes}) bytes");
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Export failed: {ex.Message}";
            _logger.LogError(ex, "Error during payment export");
        }

        return result;
    }

    public async Task<BulkExportResult> ExportPaymentsToCsvStreamingAsync(Stream outputStream, PaymentExportFilter filter, int batchSize = 1000, CancellationToken cancellationToken = default)
    {
        var result = new BulkExportResult();

        try
        {
            var totalCount = await _exportRepository.GetPaymentsCountAsync(filter, cancellationToken);

            if (totalCount == 0)
            {
                result.Success = false;
                result.ErrorMessage = "No payments found matching the filter criteria";
                return result;
            }

            using var writer = new StreamWriter(outputStream, leaveOpen: true);

            await writer.WriteLineAsync("Payment ID, Loan ID, Customer Name, Amount, Payment Date, User Name, Loan Principal, Loan Interest Rate".AsMemory(), cancellationToken);

            var startPosition = outputStream.Position;
            var processedRecords = 0;

            var payments = await _exportRepository.GetPaymentsForExportAsync(filter, cancellationToken);

            foreach (var payment in payments)
            {
                var line = $"{payment.PaymentId}," +
                        $"{payment.LoanId}," +
                        $"{_helper.EscapeCsvValue(payment.CustomerName)}," +
                        $"{payment.Amount:F2}," +
                        $"{payment.PaymentDate:yyyy-MM-dd}," +
                        $"{_helper.EscapeCsvValue(payment.UserName)}," +
                        $"{payment.LoanPrincipal:F2}," +
                        $"{payment.LoanInterestRate:F2},";

                await writer.WriteLineAsync(line.AsMemory(), cancellationToken);
                processedRecords++;

                if (processedRecords % batchSize == 0)
                    await writer.FlushAsync();
            }

            await writer.FlushAsync();
            var endPosition = outputStream.Position;

            result.Success = true;
            result.TotalRecords = processedRecords;
            result.FileSizeBytes = endPosition - startPosition;

            _logger.LogInformation($"Successfully exported {result.TotalRecords} payments in streaming mode");


        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Streaming export failed: {ex.Message}";
            _logger.LogError(ex, "Error during streaming payment export");
        }

        return result;
    }
}
