using System;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Repositories;

public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetAll();
    Task<Payment> Create(Payment payment);
    Task<Payment?> GetById(int id);
    Task Delete(int id);
}
