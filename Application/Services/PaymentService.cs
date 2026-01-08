using System;
using LendingApi.Application.Repositories;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;

    public PaymentService(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreatePayment(Payment payment)
    {
        await _repository.Create(payment);
        return true;
    }

    public async Task<bool> DeletePayment(int id)
    {
        var payment = await _repository.GetById(id);
        if (payment == null) return false;

        await _repository.Delete(payment.Id);
        return true;
    }

    public async Task<IEnumerable<Payment>> GetAllPayments()
    {
        return await _repository.GetAll();
    }

    public async Task<Payment?> GetPaymentById(int id)
    {
        return await _repository.GetById(id);
    }
}
