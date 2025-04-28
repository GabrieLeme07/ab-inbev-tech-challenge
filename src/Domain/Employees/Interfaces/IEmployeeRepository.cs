using Ab.Inbev.Service.Domain.Employees.Entities;

namespace Ab.Inbev.Service.Domain.Employees.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByEmailAsync(string email);
    Task AddAsync(Employee employee);
    Task Update(Employee employee);
    Task Remove(Employee employee);
    Task<Employee?> GetByDocumentNumberAsync(string documentNumber);
}
