using Ab.Inbev.Service.Domain.Auth.DTOs;
using Ab.Inbev.Service.Domain.Employees.Entities;

namespace Ab.Inbev.Service.Domain.Auth.Mappers;
public static class AuthMappingExtensions
{
    public static Employee ToEntity(this RegisterDirectorDto dto)
    {
        return new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            DocumentNumber = dto.DocumentNumber,
            DateOfBirth = dto.DateOfBirth,
            Role = Employees.Enums.Role.Director,
            PhoneNumbers = dto.PhoneNumbers.Select(n => new PhoneNumber { Number = n }).ToList()
        };
    }
}
