using Ab.Inbev.Service.Domain.Employees.Enums;

namespace Ab.Inbev.Service.Domain.Employees.DTOs;
public record EmployeeDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string DocumentNumber { get; init; } = null!;
    public DateTime DateOfBirth { get; init; }
    public Role Role { get; init; }
    public Guid? ManagerId { get; init; }
    public List<string> PhoneNumbers { get; init; } = [];
}
