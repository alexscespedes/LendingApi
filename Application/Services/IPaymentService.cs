using System;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Services;

public interface IPaymentService
{
    Task<bool> CreatePayment(Payment payment);
    Task<Payment?> GetPaymentById(int id);
    Task<IEnumerable<Payment>> GetAllPayments();
    Task<bool> DeletePayment(int id);
}
