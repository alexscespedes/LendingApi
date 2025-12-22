using System;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAll();
    Task<Customer> Create(Customer customer);
    Task<Customer?> GetById(int id);
    Task<IEnumerable<Customer>> SearchByName(string name);
    Task<bool> EmailExists(string email);
    Task<Customer> Update(int id, Customer customer);
    Task Delete(int customerId);
}
