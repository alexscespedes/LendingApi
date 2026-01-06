using System;
using System.ComponentModel.DataAnnotations;
using LendingApi.Core.Enums;

namespace LendingApi.Core.Entities;

public class Loan
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermsMonth { get; set; }
    public DateTime StartDate { get; set; }
    public LoanStatus LoanStatus { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
