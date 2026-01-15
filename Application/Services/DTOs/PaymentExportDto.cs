namespace LendingApi.Application.Services.DTOs;

public record PaymentExportDto
{
    public int PaymentId { get; set; }
    public int LoanId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string UserName { get; set; } = string.Empty;
    public decimal LoanPrincipal { get; set; }
    public decimal LoanInterestRate { get; set; }
}
