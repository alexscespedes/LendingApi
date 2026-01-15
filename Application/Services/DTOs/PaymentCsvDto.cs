namespace LendingApi.Application.Services.DTOs;

public record PaymentCsvDto
{
    public int LoanId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
}
