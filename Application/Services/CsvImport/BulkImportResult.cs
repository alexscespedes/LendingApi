using System;

namespace LendingApi.Application.Services.CsvImport;

public class BulkImportResult
{
    public int TotalRecords { get; set; }
    public int SuccessfulRecords { get; set; }
    public int FailedRecords { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}
