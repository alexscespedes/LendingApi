using System;
using LendingApi.Application.Services.DTOs;

namespace LendingApi.Application.Services.CsvExport;

public interface IPaymentBulkExportService
{
    Task<BulkExportResult> ExportPaymentsToCsvAsync(
        Stream outputStream,
        PaymentExportFilter filter,
        CancellationToken cancellationToken = default);

    Task<BulkExportResult> ExportPaymentsToCsvStreamingAsync(
        Stream outputStream,
        PaymentExportFilter filter,
        int batchSize = 1000,
        CancellationToken cancellationToken = default);
}
