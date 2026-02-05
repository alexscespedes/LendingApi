using System;
using LendingApi.Application.Repositories;
using LendingApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LendingApi.Data.SqlDatabase;

public class SqlLoanRepository : ILoanRepository
{
    private readonly AppDbContext _context;

    public SqlLoanRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Loan> Create(Loan loan)
    {
        var result = await _context.Loans.AddAsync(loan);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task Delete(int id)
    {
        var result = await _context.Loans
            .FirstOrDefaultAsync(l => l.Id == id);

        if (result != null)
        {
            _context.Loans.Remove(result);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Loan>> GetAll()
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.User)
            .ToListAsync();
    }

    public async Task<Loan?> GetById(int id)
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Loan> Update(int id, Loan loan)
    {
        var result = await _context.Loans
            .FirstOrDefaultAsync(l => l.Id == id);

        if (result == null) return null!;

        result.PrincipalAmount = loan.PrincipalAmount;
        result.InterestRate = loan.InterestRate;
        result.TermsMonth = loan.TermsMonth;
        result.LoanStatus = loan.LoanStatus;

        await _context.SaveChangesAsync();

        return result;
    }
}
