using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LendingApi.Application;
using LendingApi.Application.Repositories;
using LendingApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LendingApi.Data.SqlDatabase;

public class SqlCustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public SqlCustomerRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Customer> Create(Customer customer)
    {
        var result = await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task Delete(int id)
    {
        var result = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (result != null)
        {
            _context.Customers.Remove(result);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _context.Customers.AnyAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Customer>> GetAll()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetById(int id)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Customer>> SearchByName(string name)
    {
        return await _context.Customers
            .Where(c => c.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    public async Task<Customer> Update(int id, Customer customer)
    {
        var result = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (result == null) return null!;

        result.FirstName = customer.FirstName;
        result.LastName = customer.LastName;
        result.Phone = customer.Phone;
        result.Email = customer.Email;
        result.Address = customer.Address;

        await _context.SaveChangesAsync();

        return result;
    }
}
