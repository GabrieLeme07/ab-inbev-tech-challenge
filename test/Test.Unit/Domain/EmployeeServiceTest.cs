using Ab.Inbev.Service.Domain.Employees.Entities;
using Ab.Inbev.Service.Domain.Employees.Enums;
using Ab.Inbev.Service.Domain.Employees.Interfaces;
using Ab.Inbev.Service.Domain.Employees.Services;
using Ab.Inbev.Service.Test.Unit.Factory;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Ab.Inbev.Service.Test.Unit.Domain;

public class EmployeeServiceTest
{
    private Mock<IEmployeeRepository> _employeeRepositoryMock;
    private Mock<IPasswordHasher<Employee>> _passwordHasherMock;

    private IEmployeeService _service;

    [SetUp]
    public void Setup()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<Employee>>();

        _service = new EmployeeService(_employeeRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Test]
    public void CreateAsync_EmptyFirstName_ThrowsArgumentException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto_EmptyFirstName();
        Assert.ThrowsAsync<ArgumentException>(async ()
            => await _service.CreateAsync(dto, Role.Director), "FirstName is required.");
    }

    [Test]
    public void CreateAsync_EmptyLastName_ThrowsArgumentException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto_EmptyLastName();
        Assert.ThrowsAsync<ArgumentException>(async ()
            => await _service.CreateAsync(dto, Role.Director), "LastName is required.");
    }

    [Test]
    public void CreateAsync_EmptyEmail_ThrowsArgumentException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto_EmptyEmail();
        Assert.ThrowsAsync<ArgumentException>(async ()
            => await _service.CreateAsync(dto, Role.Director), "Email is required.");
    }

    [Test]
    public void CreateAsync_EmptyDocument_ThrowsArgumentException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto_EmptyDocument();
        Assert.ThrowsAsync<ArgumentException>(async ()
            => await _service.CreateAsync(dto, Role.Director), "DocumentNumber is required.");
    }

    [Test]
    public void CreateAsync_InvalidPhones_ThrowsArgumentException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto_InvalidPhones();
        Assert.ThrowsAsync<ArgumentException>(async ()
            => await _service.CreateAsync(dto, Role.Director), "At least two phone numbers are required.");
    }

    [Test]
    public void CreateAsync_UnderAge_ThrowsArgumentException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto_UnderAge();
        Assert.ThrowsAsync<ArgumentException>(async ()
            => await _service.CreateAsync(dto, Role.Director), "Employee must be at least 18 years old.");
    }

    [Test]
    public void CreateAsync_CreatorRoleTooLow_ThrowsUnauthorizedAccessException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto(Role.Director);
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _service.CreateAsync(dto, Role.Employee));
    }

    [Test]
    public async Task CreateAsync_DuplicateDocument_ThrowsArgumentException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto(Role.Employee);
        _employeeRepositoryMock.Setup(r => r.GetByDocumentNumberAsync(dto.DocumentNumber)).ReturnsAsync(new Employee());
        _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        Assert.ThrowsAsync<ArgumentException>(async ()
            => await _service.CreateAsync(dto, Role.Director), "An employee with this document number already exists.");
    }

    [Test]
    public async Task CreateAsync_DuplicateEmail_ThrowsArgumentException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto(Role.Employee);
        _employeeRepositoryMock.Setup(r => r.GetByDocumentNumberAsync(dto.DocumentNumber)).ReturnsAsync((Employee)null!);
        _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync([new Employee { Email = dto.Email }]);

        Assert.ThrowsAsync<ArgumentException>(async () => await _service.CreateAsync(dto, Role.Director), "An employee with this email already exists.");
    }

    [Test]
    public async Task CreateAsync_ManagerNotFound_ThrowsArgumentException()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto(Role.Employee);
        _employeeRepositoryMock.Setup(r => r.GetByDocumentNumberAsync(dto.DocumentNumber)).ReturnsAsync((Employee)null!);
        _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Employee>());
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(dto.ManagerId.Value)).ReturnsAsync((Employee)null!);

        Assert.ThrowsAsync<ArgumentException>(async () => await _service.CreateAsync(dto, Role.Director), "Manager not found.");
    }

    [Test]
    public async Task CreateAsync_ValidRequest_AddsEmployeeAndReturnsDto()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto(Role.Employee);
        var manager = new Employee { Id = dto.ManagerId.Value, Email = "mgr@company.com" };

        _employeeRepositoryMock.Setup(r => r.GetByDocumentNumberAsync(dto.DocumentNumber)).ReturnsAsync((Employee)null!);
        _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Employee>());
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(dto.ManagerId.Value)).ReturnsAsync(manager);
        _passwordHasherMock.Setup(h => h.HashPassword(It.IsAny<Employee>(), dto.Password)).Returns("hashed_pw");

        Employee captured = null!;
        _employeeRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Employee>()))
            .Callback<Employee>(e => captured = e)
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto, Role.Director);

        _employeeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
        _passwordHasherMock.Verify(h => h.HashPassword(captured, dto.Password), Times.Once);

        Assert.Multiple(() =>
        {
            Assert.That(dto.FirstName, Is.EqualTo(result.FirstName));
            Assert.That(dto.LastName, Is.EqualTo(result.LastName));
            Assert.That(dto.Email, Is.EqualTo(result.Email));
            Assert.That(dto.DocumentNumber, Is.EqualTo(result.DocumentNumber));
            Assert.That(dto.Role, Is.EqualTo(result.Role));
            Assert.That(manager.Id, Is.EqualTo(result.ManagerId));
            Assert.That(result.PhoneNumbers, Is.EquivalentTo(dto.PhoneNumbers));
        });
    }

    [Test]
    public async Task CreateAsync_ValidRequestWithoutManager_AddsEmployeeAndReturnsDto()
    {
        var dto = EmployeeFactory.BuildCreateEmployeeDto(Role.Employee, false);

        _employeeRepositoryMock.Setup(r => r.GetByDocumentNumberAsync(dto.DocumentNumber)).ReturnsAsync((Employee)null!);
        _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync([]);
        _passwordHasherMock.Setup(h => h.HashPassword(It.IsAny<Employee>(), dto.Password)).Returns("hashed_pw");

        _employeeRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Employee>()));

        var result = await _service.CreateAsync(dto, Role.Director);

        _employeeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(dto.Email, Is.EqualTo(result.Email));
            Assert.That(result.ManagerId, Is.Null);
        });
    }
}
