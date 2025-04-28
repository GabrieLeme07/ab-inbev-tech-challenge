namespace Ab.Inbev.Service.Domain.Auth.DTOs;

public record RegisterDirectorDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string DocumentNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public List<string> PhoneNumbers { get; set; } = new List<string>();
    public string Password { get; set; } = null!;
}
