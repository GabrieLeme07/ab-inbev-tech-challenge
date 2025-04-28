using System.ComponentModel.DataAnnotations;

namespace Ab.Inbev.Service.Domain.Employees.Entities;

public class PhoneNumber
{
    [Key]
    public int Id { get; set; }
    public string Number { get; set; } = null!;
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}
