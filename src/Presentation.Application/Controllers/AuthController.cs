using Ab.Inbev.Service.Domain.Auth.DTOs;
using Ab.Inbev.Service.Domain.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ab.Inbev.Service.Presentation.Application.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
        => _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            var result = await _authService.AuthenticateAsync(dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials.");
        }
    }

    [HttpPost("register-director")]
    public async Task<IActionResult> RegisterDirector([FromBody] RegisterDirectorDto dto)
    {
        try
        {
            var result = await _authService.RegisterDirectorAsync(dto);
            return Created(string.Empty, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
