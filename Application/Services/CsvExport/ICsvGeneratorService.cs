using System;
using LendingApi.Application.Services.DTOs;

namespace LendingApi.Application.Services.CsvExport;

public interface ICsvGeneratorService
{
    Task WriteCsvAsync<T>(Stream stream, IEnumerable<T> data, CancellationToken cancellationToken = default);
    Task WritePaymentCsvAsync(Stream stream, IEnumerable<PaymentExportDto> payments, CancellationToken cancellationToken = default);
}
