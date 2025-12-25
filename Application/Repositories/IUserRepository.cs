using System;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsername(string username);
    Task<User?> Create(User user);
    Task<bool> UsernameExists(string username);
    Task SaveChanges();

}
