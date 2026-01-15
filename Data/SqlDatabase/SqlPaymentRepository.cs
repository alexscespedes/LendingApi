using System;
using LendingApi.Application.Repositories;
using LendingApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LendingApi.Data.SqlDatabase;

public class SqlPaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public SqlPaymentRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Payment> Create(Payment payment)
    {
        var result = await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task Delete(int id)
    {
        var result = await _context.Payments
            .FirstOrDefaultAsync(p => p.Id == id);

        if (result != null)
        {
            _context.Payments.Remove(result);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Payment>> GetAll()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<Payment?> GetById(int id)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
    }
}
