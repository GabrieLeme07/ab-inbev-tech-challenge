using Ab.Inbev.Service.Domain.Employees.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ab.Inbev.Service.Domain.Employees.Entities;

public class Employee
{
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string DocumentNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public Role Role { get; set; }

    public Guid? ManagerId { get; set; }
    public Employee? Manager { get; set; }
    public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
    public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
    public string PasswordHash { get; set; } = null!;
}
