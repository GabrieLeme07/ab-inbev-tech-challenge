namespace Ab.Inbev.Service.Domain.Auth.DTOs;

public record AuthResultDto
{
    public string Token { get; init; } = null!;
    public DateTime ExpiresAt { get; init; }
}
