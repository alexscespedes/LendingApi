using System;
using LendingApi.Application.Services.DTOs;

namespace LendingApi.Data.SqlDatabase.Export;

public interface IPaymentExportRepository
{
    Task<IEnumerable<PaymentExportDto>> GetPaymentsForExportAsync(
        PaymentExportFilter filter,
        CancellationToken cancellationToken = default);

    Task<int> GetPaymentsCountAsync(
        PaymentExportFilter filter,
        CancellationToken cancellationToken = default);
}
