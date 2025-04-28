using Ab.Inbev.Service.Domain.Employees.DTOs;
using Ab.Inbev.Service.Domain.Employees.Enums;

namespace Ab.Inbev.Service.Domain.Employees.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, Role creatorRole);
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto> GetByIdAsync(Guid id);
    Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto dto, Role creatorRole);
    Task DeleteAsync(Guid id, Role creatorRole);
}
