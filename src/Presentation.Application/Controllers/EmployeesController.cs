using Ab.Inbev.Service.Domain.Employees.DTOs;
using Ab.Inbev.Service.Domain.Employees.Enums;
using Ab.Inbev.Service.Domain.Employees.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ab.Inbev.Service.Presentation.Application.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
        => _service = service;

    private Role GetCreatorRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        return Enum.TryParse<Role>(roleClaim, out var role) ? role : Role.Employee;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        var creatorRole = GetCreatorRole();
        var created = await _service.CreateAsync(dto, creatorRole);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeDto dto)
    {
        var creatorRole = GetCreatorRole();
        var updated = await _service.UpdateAsync(id, dto, creatorRole);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var creatorRole = GetCreatorRole();
        await _service.DeleteAsync(id, creatorRole);
        return NoContent();
    }
}
