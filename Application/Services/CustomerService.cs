using System;
using System.Text.RegularExpressions;
using LendingApi.Application.Repositories;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateCustomer(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.FirstName))
            return false;
        
        if (!IsValidEmail(customer.Email!))
            return false;

        var emailExisits = await _repository.EmailExists(customer.Email!);

        if (emailExisits)
            return false;

        await _repository.Create(customer);
        return true;
    }

    public async Task<bool> DeleteCustomer(int id)
    {
        var customer = await _repository.GetById(id);
        if (customer == null)
            return false;
        
        await _repository.Delete(customer.Id);
        return true;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomers()
    {
        return await _repository.GetAll();
    }

    public async Task<Customer?> GetCustomerById(int id)
    {
        return await _repository.GetById(id);
    }

    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    public async Task<IEnumerable<Customer>> SearchCustomersByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Enumerable.Empty<Customer>();

        return await _repository.SearchByName(name);
    }

    public async Task<bool> UpdateCustomer(int id, Customer customer)
    {
        var existingCustomer = await _repository.GetById(id);
        if (existingCustomer == null)
            return false;

        if (string.IsNullOrWhiteSpace(existingCustomer.FirstName))
            return false;
        
        if (!IsValidEmail(existingCustomer.Email!))
            return false;

        var emailExisits = await _repository.EmailExists(existingCustomer.Email!);

        if (emailExisits && !existingCustomer.Email!.Equals(customer.Email, StringComparison.OrdinalIgnoreCase))
            return false;

        existingCustomer.FirstName = customer.FirstName;
        existingCustomer.LastName = customer.LastName;
        existingCustomer.Phone = customer.Phone;
        existingCustomer.Email = customer.Email;
        existingCustomer.Address = customer.Address;
        
        await _repository.Update(existingCustomer.Id, existingCustomer);

        return true;
    }
}
