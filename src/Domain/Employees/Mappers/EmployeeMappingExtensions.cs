using Ab.Inbev.Service.Domain.Employees.DTOs;
using Ab.Inbev.Service.Domain.Employees.Entities;

namespace Ab.Inbev.Service.Domain.Employees.Mappers;

public static class EmployeeMappingExtensions
{
    public static Employee ToEntity(this CreateEmployeeDto dto)
    {
        return new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            DocumentNumber = dto.DocumentNumber,
            DateOfBirth = dto.DateOfBirth,
            Role = dto.Role,
            ManagerId = dto.ManagerId,
            PhoneNumbers = dto.PhoneNumbers.Select(n => new PhoneNumber { Number = n }).ToList()
        };
    }

    public static void UpdateFromDto(this Employee entity, UpdateEmployeeDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.FirstName))
            entity.FirstName = dto.FirstName;
        if (!string.IsNullOrWhiteSpace(dto.LastName))
            entity.LastName = dto.LastName;
        if (!string.IsNullOrWhiteSpace(dto.Email))
            entity.Email = dto.Email;
        if (dto.PhoneNumbers != null && dto.PhoneNumbers.Count >= 2)
        {
            entity.PhoneNumbers.Clear();
            foreach (var number in dto.PhoneNumbers)
                entity.PhoneNumbers.Add(new PhoneNumber { Number = number });
        }
        if (dto.Role.HasValue)
            entity.Role = dto.Role.Value;
        if (dto.ManagerId.HasValue)
            entity.ManagerId = dto.ManagerId.Value;
    }

    public static EmployeeDto ToDto(this Employee entity)
    {
        return new EmployeeDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            DocumentNumber = entity.DocumentNumber,
            DateOfBirth = entity.DateOfBirth,
            Role = entity.Role,
            ManagerId = entity.ManagerId,
            PhoneNumbers = entity.PhoneNumbers.Select(p => p.Number).ToList()
        };
    }
}
