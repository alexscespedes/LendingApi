using System;
using LendingApi.Application.Helpers;
using LendingApi.Application.Services.DTOs;

namespace LendingApi.Application.Services.CsvExport;

public class CsvGeneratorService : ICsvGeneratorService
{
    private readonly CsvExportHelper _helper;

    public CsvGeneratorService(CsvExportHelper helper)
    {
        _helper = helper;
    }
    public async Task WriteCsvAsync<T>(Stream stream, IEnumerable<T> data, CancellationToken cancellationToken = default)
    {
        using var writer = new StreamWriter(stream, leaveOpen: true);

        var properties = typeof(T).GetProperties();

        // Write header
        var header = string.Join(",", properties.Select(p => _helper.EscapeCsvValue(p.Name)));
        await writer.WriteLineAsync(header.AsMemory(), cancellationToken);

        // Write data rows
        foreach (var item in data)
        {
            var values = properties.Select(p =>
            {
                var value = p.GetValue(item);
                return _helper.EscapeCsvValue(value?.ToString() ?? string.Empty);
            });

            var line = string.Join(",", values);
            await writer.WriteLineAsync(line.AsMemory(), cancellationToken);
        }

        await writer.FlushAsync();
    }



    public async Task WritePaymentCsvAsync(Stream stream, IEnumerable<PaymentExportDto> payments, CancellationToken cancellationToken = default)
    {
        using var writer = new StreamWriter(stream, leaveOpen: true);

        // Write header
        await writer.WriteLineAsync("Payment ID, Loan ID, Customer Name, Amount, Payment Date, User Name, Loan Principal, Loan Interest Rate".AsMemory(), cancellationToken);

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
        }

        await writer.FlushAsync();
    }
}
