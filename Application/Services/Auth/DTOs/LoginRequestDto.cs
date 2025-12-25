namespace LendingApi.Application.Services.Auth.DTOs;

public record LoginRequestDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
