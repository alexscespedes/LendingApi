using System;
using LendingApi.Application.Helpers;
using LendingApi.Application.Repositories;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _repository;
    private readonly LoanHelper _helper;

    public LoanService(ILoanRepository repository, LoanHelper helper)
    {
        _repository = repository;
        _helper = helper;
    }

    public async Task<bool> CreateLoan(Loan loan)
    {
        _helper.LoanBusinessalidation(loan);
        
        await _repository.Create(loan);
        return true;
    }

    public async Task<bool> DeleteLoan(int id)
    {
        var customer = await _repository.GetById(id);
        if (customer == null) return false;

        await _repository.Delete(customer.Id);
        return true;
    }

    public async Task<IEnumerable<Loan>> GetAllLoans()
    {
        return await _repository.GetAll();
    }

    public async Task<Loan?> GetLoanById(int id)
    {
        return await _repository.GetById(id);
    }

    public async Task<bool> UpdateLoan(int id, Loan loan)
    {
        var existingLoan = await _repository.GetById(id);
        if (existingLoan == null)
            return false;
        
        _helper.LoanBusinessalidation(loan);

        existingLoan.PrincipalAmount = loan.PrincipalAmount;
        existingLoan.InterestRate = loan.InterestRate;
        existingLoan.TermsMonth = loan.TermsMonth;
        existingLoan.LoanStatus = loan.LoanStatus;

        await _repository.Update(existingLoan.Id, existingLoan);
        return true;
    }
}
