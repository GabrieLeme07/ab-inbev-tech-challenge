using Ab.Inbev.Service.Domain.Auth.Interfaces;
using Ab.Inbev.Service.Domain.Auth.Services;
using Ab.Inbev.Service.Domain.Employees.Entities;
using Ab.Inbev.Service.Domain.Employees.Interfaces;
using Ab.Inbev.Service.Domain.Employees.Services;
using Infrastructure.Data.Repository;
using Microsoft.AspNetCore.Identity;

namespace Ab.Inbev.Service.Presentation.Application.Extensions.ServiceCollectionExtensions;

public static class AddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAuthService, AuthService>();

        // Repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();

        return services;
    }
}