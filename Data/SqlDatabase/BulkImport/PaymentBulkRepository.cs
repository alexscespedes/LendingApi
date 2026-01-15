using System;
using System.Data;
using LendingApi.Application.Services.DTOs;
using LendingApi.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LendingApi.Data.SqlDatabase.BulkImport;

public class PaymentBulkRepository : IPaymentBulkRepository
{
    private readonly AppDbContext _context;
    public PaymentBulkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task BulkInsertAsync(IEnumerable<Payment> payments, CancellationToken cancellationToken = default)
    {
        await _context.Payments.AddRangeAsync(payments, cancellationToken);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ValidateLoanExistsAsync(int loanId, CancellationToken cancellationToken = default)
    {
        return await _context.Loans.AnyAsync(l => l.Id == loanId, cancellationToken);
    }

    public async Task<bool> ValidateUserExistsAsync(int UserId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AnyAsync(u => u.Id == UserId, cancellationToken);
    }

    public async Task<IEnumerable<int>> GetExistingLoanIdsAsync(IEnumerable<int> loansIds, CancellationToken cancellationToken = default)
    {
        return await _context.Loans
            .Where(l => loansIds.Contains(l.Id))
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<int>> GetExistingUserIdsAsync(IEnumerable<int> userIds, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);
    }
}
