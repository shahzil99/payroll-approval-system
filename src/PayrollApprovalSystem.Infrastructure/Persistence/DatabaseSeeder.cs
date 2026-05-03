using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PayrollApprovalSystem.Domain.Entities;

namespace PayrollApprovalSystem.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(AppDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Departments.AnyAsync())
        {
            _logger.LogInformation("Database already seeded, skipping.");
            return;
        }

        _logger.LogInformation("Seeding database with initial data...");

        var departments = new[]
        {
            new Department(Guid.Parse("10000000-0000-0000-0000-000000000001"), "Human Resources"),
            new Department(Guid.Parse("10000000-0000-0000-0000-000000000002"), "Engineering"),
            new Department(Guid.Parse("10000000-0000-0000-0000-000000000003"), "Finance"),
            new Department(Guid.Parse("10000000-0000-0000-0000-000000000004"), "Marketing"),
            new Department(Guid.Parse("10000000-0000-0000-0000-000000000005"), "Operations"),
        };

        await _context.Departments.AddRangeAsync(departments);
        await _context.SaveChangesAsync();

        var adminEmployee = new Employee(
            Guid.Parse("20000000-0000-0000-0000-000000000001"),
            "Admin",
            "User",
            "admin@payroll.com",
            departments[0].Id);

        await _context.Employees.AddAsync(adminEmployee);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Database seeded with {DeptCount} departments and 1 admin employee.", departments.Length);
    }
}
