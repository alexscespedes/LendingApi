using System;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Services;

public interface ICustomerService
{
    Task<bool> CreateCustomer(Customer customer);
    Task<Customer?> GetCustomerById(int id);
    Task<IEnumerable<Customer>> GetAllCustomers();
    Task<IEnumerable<Customer>> SearchCustomersByName(string name);
    Task<bool> UpdateCustomer(int id, Customer customer);
    Task<bool> DeleteCustomer(int id);
}
