using System;
using LendingApi.Application.Services.Auth.DTOs;

namespace LendingApi.Application.Services.Auth;

public interface IAuthService
{
    Task<bool> Register(RegisterRequestDto requestDto);
    Task<bool> Login(LoginRequestDto requestDto);

}
