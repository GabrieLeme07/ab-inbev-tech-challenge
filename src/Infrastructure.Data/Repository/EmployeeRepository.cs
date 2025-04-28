using Ab.Inbev.Service.Domain.Employees.Entities;
using Ab.Inbev.Service.Domain.Employees.Interfaces;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository;
public class EmployeeRepository(AppDbContext context) : RepositoryBase<Employee>(context), IEmployeeRepository
{
    public async Task<Employee?> GetByIdAsync(Guid id)
        => await _context.Set<Employee>()
            .Include(e => e.Manager)
            .Include(e => e.Subordinates)
            .Include(e => e.PhoneNumbers)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _context.Set<Employee>()
            .Include(e => e.Manager)
            .Include(e => e.PhoneNumbers)
            .ToListAsync();

    public async Task<Employee?> GetByEmailAsync(string email)
        => await _context.Set<Employee>()
            .Include(e => e.Manager)
            .Include(e => e.PhoneNumbers)
            .FirstOrDefaultAsync(e => e.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    public new async Task AddAsync(Employee employee)
        => await base.AddAsync(employee);

    public new async Task Update(Employee employee)
        => await base.Update(employee);

    public new async Task Remove(Employee employee)
        => await base.Remove(employee);

    public async Task<Employee?> GetByDocumentNumberAsync(string documentNumber)
        => await _context.Set<Employee>()
            .FirstOrDefaultAsync(e => e.DocumentNumber == documentNumber);
}
