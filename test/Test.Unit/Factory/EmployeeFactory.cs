using Ab.Inbev.Service.Domain.Employees.DTOs;
using Ab.Inbev.Service.Domain.Employees.Enums;
using Bogus;
using Bogus.Extensions.Brazil;

namespace Ab.Inbev.Service.Test.Unit.Factory;
public static class EmployeeFactory
{
    public static CreateEmployeeDto BuildCreateEmployeeDto_EmptyFirstName()
    {
        var response = new Faker<CreateEmployeeDto>()
            .CustomInstantiator(f => new CreateEmployeeDto
            {
                FirstName = "",
                LastName = f.Person.LastName,
                Email = f.Person.Email,
                DocumentNumber = f.Person.Cpf(false),
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                PhoneNumbers = [f.Phone.PhoneNumber(), f.Phone.PhoneNumber()],
                Role = Role.Employee
            }
            );

        return response;
    }

    public static CreateEmployeeDto BuildCreateEmployeeDto_EmptyLastName()
    {
        var response = new Faker<CreateEmployeeDto>()
            .CustomInstantiator(f => new CreateEmployeeDto
            {
                FirstName = f.Person.FirstName,
                LastName = "",
                Email = f.Person.Email,
                DocumentNumber = f.Person.Cpf(false),
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                PhoneNumbers = [f.Phone.PhoneNumber(), f.Phone.PhoneNumber()],
                Role = Role.Employee
            }
            );

        return response;
    }

    public static CreateEmployeeDto BuildCreateEmployeeDto_EmptyEmail()
    {
        var response = new Faker<CreateEmployeeDto>()
            .CustomInstantiator(f => new CreateEmployeeDto
            {
                FirstName = f.Person.FirstName,
                LastName = f.Person.LastName,
                Email = "",
                DocumentNumber = f.Person.Cpf(false),
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                PhoneNumbers = [f.Phone.PhoneNumber(), f.Phone.PhoneNumber()],
                Role = Role.Employee
            }
            );

        return response;
    }

    public static CreateEmployeeDto BuildCreateEmployeeDto_EmptyDocument()
    {
        var response = new Faker<CreateEmployeeDto>()
            .CustomInstantiator(f => new CreateEmployeeDto
            {
                FirstName = f.Person.FirstName,
                LastName = f.Person.LastName,
                Email = f.Person.Email,
                DocumentNumber = "",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                PhoneNumbers = [f.Phone.PhoneNumber(), f.Phone.PhoneNumber()],
                Role = Role.Employee
            }
            );

        return response;
    }

    public static CreateEmployeeDto BuildCreateEmployeeDto_InvalidPhones()
    {
        var response = new Faker<CreateEmployeeDto>()
            .CustomInstantiator(f => new CreateEmployeeDto
            {
                FirstName = f.Person.FirstName,
                LastName = f.Person.LastName,
                Email = f.Person.Email,
                DocumentNumber = f.Person.Cpf(false),
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                PhoneNumbers = [f.Phone.PhoneNumber()],
                Role = Role.Employee
            });

        return response;
    }

    public static CreateEmployeeDto BuildCreateEmployeeDto_UnderAge()
    {
        var response = new Faker<CreateEmployeeDto>()
            .CustomInstantiator(f => new CreateEmployeeDto
            {
                FirstName = f.Person.FirstName,
                LastName = f.Person.LastName,
                Email = f.Person.Email,
                DocumentNumber = f.Person.Cpf(false),
                DateOfBirth = DateTime.UtcNow.AddYears(-17),
                PhoneNumbers = [f.Phone.PhoneNumber(), f.Phone.PhoneNumber()],
                Role = Role.Employee
            });

        return response;
    }

    public static CreateEmployeeDto BuildCreateEmployeeDto(Role role, bool withManager = true)
    {
        var response = new Faker<CreateEmployeeDto>()
            .CustomInstantiator(f => new CreateEmployeeDto
            {
                FirstName = f.Person.FirstName,
                LastName = f.Person.LastName,
                Email = f.Person.Email,
                DocumentNumber = f.Person.Cpf(false),
                DateOfBirth = DateTime.UtcNow.AddYears(-18),
                PhoneNumbers = [f.Phone.PhoneNumber(), f.Phone.PhoneNumber()],
                Role = role,
                ManagerId = withManager ? Guid.NewGuid() : null,
                Password = f.Internet.Password()
            });

        return response;
    }
}
