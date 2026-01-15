using System;
using LendingApi.Application.Services.DTOs;
using LendingApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LendingApi.Data.SqlDatabase.Export;

public class PaymentExportRepository : IPaymentExportRepository
{
    private readonly AppDbContext _context;

    public PaymentExportRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<int> GetPaymentsCountAsync(PaymentExportFilter filter, CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(filter);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<PaymentExportDto>> GetPaymentsForExportAsync(PaymentExportFilter filter, CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(filter);

        return await query
            .AsNoTracking()
            .Select(p => new PaymentExportDto
            {
                PaymentId = p.Id,
                LoanId = p.LoanId,
                CustomerName = p.Loan!.Customer!.FirstName + " " + p.Loan!.Customer!.LastName ?? "Unknown",
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                UserName = p.User!.Username ?? "Unknown",
                LoanPrincipal = p.Loan.PrincipalAmount,
                LoanInterestRate = p.Loan.InterestRate
            })
            .ToListAsync();
    }

    private IQueryable<Payment> BuildQuery(PaymentExportFilter filter)
    {
        var query = _context.Payments
            .Include(p => p.Loan)
                .ThenInclude(l => l!.Customer)
            .Include(p => p.User)
            .AsQueryable();

        if (filter.LoanId.HasValue)
            query = query.Where(p => p.LoanId == filter.LoanId.Value);

        if (filter.UserId.HasValue)
            query = query.Where(p => p.UserId == filter.UserId.Value);
        
        if (filter.StartDate.HasValue)
            query = query.Where(p => p.PaymentDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(p => p.PaymentDate <= filter.EndDate.Value);

        if (filter.MinAmount.HasValue)
            query = query.Where(p => p.Amount >= filter.MinAmount.Value);

        if (filter.MaxAmount.HasValue)
            query = query.Where(p => p.Amount <= filter.MaxAmount.Value);

        if (filter.CustomerIds != null && filter.CustomerIds.Any())
            query = query.Where(p => filter.CustomerIds.Contains(p.Loan!.CustomerId));

        return query.OrderBy(p => p.PaymentDate).ThenBy(p => p.Id);
    }
}
