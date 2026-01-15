using System;

namespace LendingApi.Application.Services.CsvImport;

public interface IPaymentBulkImportService
{
    Task<BulkImportResult> ImportPaymentsFromCsvAsync(Stream csvStream, CancellationToken cancellationToken = default);
}
