namespace Ab.Inbev.Service.Domain.Auth.DTOs;
public record LoginRequestDto
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
