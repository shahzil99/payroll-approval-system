using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayrollApprovalSystem.Domain.Entities;

namespace PayrollApprovalSystem.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(AppDbContext context, IConfiguration configuration, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        var environment = _configuration["ASPNETCORE_ENVIRONMENT"]
            ?? _configuration["Environment"]
            ?? "Production";

        if (!string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Environment is '{Env}', skipping seed.", environment);
            return;
        }

        if (await _context.Departments.AnyAsync())
        {
            _logger.LogInformation("Database already seeded, skipping.");
            return;
        }

        _logger.LogInformation("Development environment detected — seeding demo data...");

        // 1. Departments
        var hrDepartment = new Department(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            "Human Resources");

        var departments = new[]
        {
            hrDepartment,
            new Department(Guid.Parse("10000000-0000-0000-0000-000000000002"), "Engineering"),
            new Department(Guid.Parse("10000000-0000-0000-0000-000000000003"), "Finance"),
            new Department(Guid.Parse("10000000-0000-0000-0000-000000000004"), "Marketing"),
            new Department(Guid.Parse("10000000-0000-0000-0000-000000000005"), "Operations"),
        };

        await _context.Departments.AddRangeAsync(departments);
        await _context.SaveChangesAsync();

        // 2. Employee
        var adminEmployee = new Employee(
            Guid.Parse("20000000-0000-0000-0000-000000000001"),
            "Admin",
            "User",
            "admin@payroll.com",
            hrDepartment.Id);

        await _context.Employees.AddAsync(adminEmployee);
        await _context.SaveChangesAsync();

        // 3. Active PayrollStructure (enables Generate Payroll flow)
        var payrollStructure = new PayrollStructure(
            Guid.Parse("30000000-0000-0000-0000-000000000001"),
            adminEmployee.Id,
            baseSalary: 50000m,
            bonus: 5000m,
            deductions: 8000m);

        await _context.PayrollStructures.AddAsync(payrollStructure);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Seeded {DeptCount} departments, 1 employee, 1 payroll structure. " +
            "Ready for Swagger flow: Login -> Generate Payroll -> Approve -> Payslip -> PDF.",
            departments.Length);
    }
}
