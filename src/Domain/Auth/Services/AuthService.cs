using Ab.Inbev.Service.Domain.Auth.DTOs;
using Ab.Inbev.Service.Domain.Auth.Interfaces;
using Ab.Inbev.Service.Domain.Auth.Mappers;
using Ab.Inbev.Service.Domain.Employees.Entities;
using Ab.Inbev.Service.Domain.Employees.Enums;
using Ab.Inbev.Service.Domain.Employees.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ab.Inbev.Service.Domain.Auth.Services;
public class AuthService : IAuthService
{
    private readonly IEmployeeRepository _repository;
    private readonly IPasswordHasher<Employee> _passwordHasher;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _signingKey;
    private readonly int _expirationHours;

    public AuthService(
        IEmployeeRepository repository,
        IPasswordHasher<Employee> passwordHasher,
        IConfiguration configuration)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _issuer = configuration["Jwt:Issuer"]!;
        _audience = configuration["Jwt:Audience"]!;
        var secret = configuration["Jwt:Secret"]!;
        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        _expirationHours = int.TryParse(configuration["Jwt:ExpirationHours"], out var hrs) ? hrs : 2;
    }

    public async Task<AuthResultDto> AuthenticateAsync(LoginRequestDto login)
    {
        var user = await _repository.GetByEmailAsync(login.Email)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid credentials.");

        return GenerateToken(user);
    }

    public async Task<AuthResultDto> RegisterDirectorAsync(RegisterDirectorDto dto)
    {
        var exists = (await _repository.GetAllAsync())
            .Any(e => e.Role == Role.Director);
        if (exists)
            throw new InvalidOperationException("A director already exists.");

        var director = dto.ToEntity();
        director.PasswordHash = _passwordHasher.HashPassword(director, dto.Password);

        await _repository.AddAsync(director);

        return GenerateToken(director);
    }

    private AuthResultDto GenerateToken(Employee user)
    {
        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString())
            };

        var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(_expirationHours);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new AuthResultDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expires
        };
    }
}
