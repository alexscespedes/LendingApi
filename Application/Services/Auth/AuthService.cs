using System;
using LendingApi.Application.Repositories;
using LendingApi.Application.Services.Auth.DTOs;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly PasswordService _passwordService;
    public AuthService(IUserRepository repository, PasswordService passwordService)
    {
        _repository = repository;
        _passwordService = passwordService;
    }

    
    public async Task<bool> Register(RegisterRequestDto requestDto)
    {
        if (await _repository.UsernameExists(requestDto.Username))
            return false;

        var user = new User
        {
            Username = requestDto.Username,
            PasswordHash = _passwordService.HashPassword(requestDto.Password)
        };

        await _repository.Create(user);
        await _repository.SaveChanges();

        return true;
    }
    
    public async Task<bool> Login(LoginRequestDto requestDto)
    {
        var user = await _repository.GetByUsername(requestDto.Username);
        if (user == null)
            return false;

        if (!_passwordService.VerifyPassword(user.PasswordHash, requestDto.Password))
            return false;
        
        return true;
    }

}
