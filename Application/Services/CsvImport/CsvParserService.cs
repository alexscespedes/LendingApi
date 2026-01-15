using System;
using System.Globalization;
using CsvHelper;
using LendingApi.Application.Services.DTOs;
using LendingApi.Data.SqlDatabase.BulkImport;

namespace LendingApi.Application.Services.CsvImport;

public class CsvParserService : ICsvParserService
{
    public async Task<IEnumerable<PaymentCsvDto>> ParsePaymentsCsvAsync(Stream csvStream)
    {
        var payments = new List<PaymentCsvDto>();

        using (var reader = new StreamReader(csvStream))
        {
            await reader.ReadLineAsync();

            string? line;
            int lineNumber = 1;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var values = line.Split(',');

                if (values.Length < 4)
                    throw new InvalidDataException($"Invalid data format at line {lineNumber}");

                try
                {
                    var payment = new PaymentCsvDto
                    {
                        LoanId = int.Parse(values[0].Trim()),
                        Amount = decimal.Parse(values[1].Trim()),
                        PaymentDate = DateTime.Parse(values[2].Trim()),
                        UserId = int.Parse(values[3].Trim())
                    };

                    payments.Add(payment);
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException($"Error parsing line {lineNumber}: {ex.Message}", ex);
                }
            }
        }

        return payments;
    }
}
