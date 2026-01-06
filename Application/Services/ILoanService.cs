using System;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Services;

public interface ILoanService
{
    Task<bool> CreateLoan(Loan loan);
    Task<Loan?> GetLoanById(int id);
    Task<IEnumerable<Loan>> GetAllLoans();
    Task<bool> UpdateLoan(int id, Loan loan);
    Task<bool> DeleteLoan(int id);
}
