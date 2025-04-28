using Ab.Inbev.Service.Domain.Employees.Enums;

namespace Ab.Inbev.Service.Domain.Employees.DTOs;

public record UpdateEmployeeDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<string> PhoneNumbers { get; set; }
    public Role? Role { get; set; }
    public Guid? ManagerId { get; set; }
    public string Password { get; set; }
}
