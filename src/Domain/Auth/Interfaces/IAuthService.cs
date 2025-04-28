using Ab.Inbev.Service.Domain.Auth.DTOs;

namespace Ab.Inbev.Service.Domain.Auth.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> AuthenticateAsync(LoginRequestDto login);
    Task<AuthResultDto> RegisterDirectorAsync(RegisterDirectorDto dto);
}
