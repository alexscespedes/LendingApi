using System;
using LendingApi.Core.Entities;

namespace LendingApi.Data.SqlDatabase.BulkImport;

public interface IPaymentBulkRepository
{
    Task BulkInsertAsync(IEnumerable<Payment> payments, CancellationToken cancellationToken = default);
    Task<bool> ValidateLoanExistsAsync(int loanId, CancellationToken cancellationToken = default);
    Task<bool> ValidateUserExistsAsync(int UserId, CancellationToken cancellationToken = default);
    Task<IEnumerable<int>> GetExistingLoanIdsAsync(IEnumerable<int> loansIds, CancellationToken cancellationToken = default);
    Task<IEnumerable<int>> GetExistingUserIdsAsync(IEnumerable<int> userIds, CancellationToken cancellationToken = default);

}
