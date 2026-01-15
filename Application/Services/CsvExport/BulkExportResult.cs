using System;

namespace LendingApi.Application.Services.CsvExport;

public class BulkExportResult
{
    public int TotalRecords { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public long FileSizeBytes { get; set; }
}
