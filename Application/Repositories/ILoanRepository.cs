using System;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Repositories;

public interface ILoanRepository
{
    Task<IEnumerable<Loan>> GetAll();
    Task<Loan> Create(Loan loan);
    Task<Loan?> GetById(int id);
    Task<Loan> Update(int id, Loan loan);
    Task Delete(int id);
}
