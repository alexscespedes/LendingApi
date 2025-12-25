using System;
using LendingApi.Application.Repositories;
using LendingApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LendingApi.Data.SqlDatabase;

public class SqlUserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public SqlUserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<User?> Create(User user)
    {
        var result = await _context.Users.AddAsync(user);
        return result.Entity;
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UsernameExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
}
