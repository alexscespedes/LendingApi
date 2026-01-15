using System;
using LendingApi.Application.Services.DTOs;

namespace LendingApi.Application.Services.CsvImport;

public interface ICsvParserService
{
    Task<IEnumerable<PaymentCsvDto>> ParsePaymentsCsvAsync(Stream csvStream);
}
