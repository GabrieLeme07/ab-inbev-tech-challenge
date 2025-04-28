using Ab.Inbev.Service.Domain.Employees.DTOs;
using Ab.Inbev.Service.Domain.Employees.Entities;
using Ab.Inbev.Service.Domain.Employees.Enums;
using Ab.Inbev.Service.Domain.Employees.Interfaces;
using Ab.Inbev.Service.Domain.Employees.Mappers;
using Microsoft.AspNetCore.Identity;

namespace Ab.Inbev.Service.Domain.Employees.Services;
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IPasswordHasher<Employee> _passwordHasher;

    public EmployeeService(IEmployeeRepository repository, IPasswordHasher<Employee> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, Role creatorRole)
    {
        // Mandatory validations
        if (string.IsNullOrWhiteSpace(dto.FirstName)) throw new ArgumentException("FirstName is required.");
        if (string.IsNullOrWhiteSpace(dto.LastName)) throw new ArgumentException("LastName is required.");
        if (string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Email is required.");
        if (string.IsNullOrWhiteSpace(dto.DocumentNumber)) throw new ArgumentException("DocumentNumber is required.");
        if (dto.PhoneNumbers == null || dto.PhoneNumbers.Count < 2) throw new ArgumentException("At least two phone numbers are required.");

        // Age validation
        var age = DateTime.UtcNow.Year - dto.DateOfBirth.Year;
        if (dto.DateOfBirth > DateTime.UtcNow.AddYears(-age)) age--;
        if (age < 18) throw new ArgumentException("Employee must be at least 18 years old.");

        // Role permission validation
        if (creatorRole < dto.Role) throw new UnauthorizedAccessException("You cannot create a user with higher permissions than yours.");

        // Unique document check
        var existingByDoc = await _repository.GetByDocumentNumberAsync(dto.DocumentNumber);
        if (existingByDoc != null) throw new ArgumentException("An employee with this document number already exists.");

        // Unique email check
        var allEmployees = await _repository.GetAllAsync();
        if (allEmployees.Any(e => e.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("An employee with this email already exists.");

        // Manager validation
        if (dto.ManagerId.HasValue)
        {
            var manager = await _repository.GetByIdAsync(dto.ManagerId.Value)
                ?? throw new ArgumentException("Manager not found.");
        }

        // Map DTO to entity
        var entity = dto.ToEntity();

        // Hash password
        entity.PasswordHash = _passwordHasher.HashPassword(entity, dto.Password);

        // Persist
        await _repository.AddAsync(entity);

        // Map entity to DTO
        return entity.ToDto();
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();
        return employees.Select(e => e.ToDto());
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee?.ToDto();
    }

    public async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto dto, Role creatorRole)
    {
        var existing = await _repository.GetByIdAsync(id) 
            ?? throw new ArgumentException("Employee not found.");

        // Permission checks
        if (creatorRole < existing.Role) throw new UnauthorizedAccessException("You cannot update a user with higher permissions than yours.");
        if (dto.Role.HasValue && creatorRole < dto.Role.Value)
            throw new UnauthorizedAccessException("You cannot assign a higher role than your own.");

        // Update fields
        if (!string.IsNullOrWhiteSpace(dto.FirstName)) existing.FirstName = dto.FirstName;
        if (!string.IsNullOrWhiteSpace(dto.LastName)) existing.LastName = dto.LastName;
        if (!string.IsNullOrWhiteSpace(dto.Email)) existing.Email = dto.Email;
        if (dto.PhoneNumbers != null && dto.PhoneNumbers.Count >= 2)
        {
            existing.PhoneNumbers.Clear();
            foreach (var num in dto.PhoneNumbers)
                existing.PhoneNumbers.Add(new PhoneNumber { Number = num });
        }
        if (dto.Role.HasValue) existing.Role = dto.Role.Value;
        if (dto.ManagerId.HasValue) existing.ManagerId = dto.ManagerId.Value;

        // Update password if provided
        if (!string.IsNullOrWhiteSpace(dto.Password))
            existing.PasswordHash = _passwordHasher.HashPassword(existing, dto.Password);

        // Persist
        await _repository.Update(existing);

        // Map to DTO
        return existing.ToDto();
    }

    public async Task DeleteAsync(Guid id, Role creatorRole)
    {
        var existing = await _repository.GetByIdAsync(id) ?? throw new ArgumentException("Employee not found.");
        if (creatorRole < existing.Role)
            throw new UnauthorizedAccessException("You cannot delete a user with higher permissions than yours.");
        await _repository.Remove(existing);
    }
}
