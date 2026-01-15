using System;
using System.ComponentModel.DataAnnotations;
using CsvHelper;
using LendingApi.Application.Services.DTOs;
using LendingApi.Core.Entities;
using LendingApi.Data.SqlDatabase.BulkImport;

namespace LendingApi.Application.Services.CsvImport;

public class PaymentBulkImportService : IPaymentBulkImportService
{

    private readonly IPaymentBulkRepository _bulkRepository;
    private readonly ICsvParserService _csvParser;
    private readonly ILogger<PaymentBulkImportService> _logger;

    public PaymentBulkImportService(
        IPaymentBulkRepository bulkRepository,
        ICsvParserService csvParser,
        ILogger<PaymentBulkImportService> logger
        )
    {
        _bulkRepository = bulkRepository;
        _csvParser = csvParser;
        _logger = logger;
    }

    public async Task<BulkImportResult> ImportPaymentsFromCsvAsync(Stream csvStream, CancellationToken cancellationToken = default)
    {
        var result = new BulkImportResult();

        try
        {
            var paymentsDtos = await _csvParser.ParsePaymentsCsvAsync(csvStream);
            result.TotalRecords = paymentsDtos.Count();

            if (!paymentsDtos.Any())
            {
                result.Errors.Add("CSV file is empty or contains only headers");
                return result;
            }

            var validationResult = await ValidateForeignKeysAsync(paymentsDtos, cancellationToken);

            if (validationResult.Errors.Any())
            {
                result.Errors.AddRange(validationResult.Errors);
                result.FailedRecords = result.TotalRecords;
                return result;
            }

            var payments = paymentsDtos.Select(dto => new Payment
            {
                LoanId = dto.LoanId,
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate,
                UserId = dto.UserId
            }).ToList();

            await _bulkRepository.BulkInsertAsync(payments, cancellationToken);

            result.SuccessfulRecords = payments.Count;
            _logger.LogInformation($"Successfully imported {result.SuccessfulRecords} payments");
        }

        catch (InvalidDataException ex)
        {
            result.Errors.Add($"CSV parsing error: {ex.Message}");
            result.FailedRecords = result.TotalRecords;
            _logger.LogError(ex, "Error parsing CSV file");
        }

        catch (Exception ex)
        {
            result.Errors.Add($"Unexpected error: {ex.Message}");
            result.FailedRecords = result.TotalRecords;
            _logger.LogError(ex, "Error during bulk import");
        }

        return result;
    }

    private async Task<ValidationResult> ValidateForeignKeysAsync(IEnumerable<PaymentCsvDto> paymentsDtos, CancellationToken cancellationToken)
    {
        var validationResult = new ValidationResult();

        var loandIds = paymentsDtos.Select(p => p.LoanId).Distinct().ToList();
        var userIds = paymentsDtos.Select(p => p.UserId).Distinct().ToList();

        var existingLoanIds = (await _bulkRepository.GetExistingLoanIdsAsync(loandIds, cancellationToken)).ToHashSet();
        var existingUserIds = (await _bulkRepository.GetExistingUserIdsAsync(userIds, cancellationToken)).ToHashSet();

        var invalidLoanIds = loandIds.Where(id => !existingLoanIds.Contains(id)).ToList();
        var invalidUserIds = userIds.Where(id => !existingUserIds.Contains(id)).ToList();

        if (invalidLoanIds.Any())
            validationResult.Errors.Add($"Invalid Loan IDs: {string.Join(", ", invalidLoanIds)}");

        if (invalidUserIds.Any())
            validationResult.Errors.Add($"Invalid User IDs: {string.Join(", ", invalidUserIds)}");

        return validationResult;

    }

    private class ValidationResult
    {
        public List<string> Errors { get; set; } = new List<string>();
    }
}
