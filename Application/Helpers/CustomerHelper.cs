using System;
using System.Text.RegularExpressions;

namespace LendingApi.Application.Helpers;

public class CustomerHelper
{
    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
}
