using System;
using Microsoft.AspNetCore.Identity;

namespace LendingApi.Application.Services.Auth;

public class PasswordService
{
    private readonly PasswordHasher<string> _hasher = new();

    public string HashPassword(string password) => 
        _hasher.HashPassword(null!, password);

    public bool VerifyPassword(string hash, string password) =>
        _hasher.VerifyHashedPassword(null!, hash, password) == PasswordVerificationResult.Success;
}
