using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Infrastructure.Repositories;
using PayrollApprovalSystem.Infrastructure.Tests;

public class EmployeeRepositoryTests
{
    private static (EmployeeRepository repo, AppDbContext db) Create()
    {
        var db = DbContextFactory.Create();
        return (new EmployeeRepository(db), db);
    }

    private static Department SeedDepartment(AppDbContext db)
    {
        var dept = new Department(Guid.NewGuid(), "Engineering");
        db.Departments.Add(dept);
        db.SaveChanges();
        return dept;
    }

    [Fact]
    public async Task AddAsync_PersistsEmployee()
    {
        var (repo, db) = Create();
        var dept = SeedDepartment(db);
        var employee = new Employee(Guid.NewGuid(), "John", "Doe", "john@example.com", dept.Id);

        await repo.AddAsync(employee);

        var result = await db.Employees.FindAsync(employee.Id);
        Assert.NotNull(result);
        Assert.Equal("John", result!.FirstName);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEmployee_WhenExists()
    {
        var (repo, db) = Create();
        var dept = SeedDepartment(db);
        var employee = new Employee(Guid.NewGuid(), "Jane", "Smith", "jane@example.com", dept.Id);
        db.Employees.Add(employee);
        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(employee.Id);

        Assert.NotNull(result);
        Assert.Equal("Jane", result!.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        var (repo, _) = Create();

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEmployees()
    {
        var (repo, db) = Create();
        var dept = SeedDepartment(db);
        db.Employees.AddRange(
            new Employee(Guid.NewGuid(), "A", "B", "a@b.com", dept.Id),
            new Employee(Guid.NewGuid(), "C", "D", "c@d.com", dept.Id));
        await db.SaveChangesAsync();

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEmployee()
    {
        var (repo, db) = Create();
        var dept = SeedDepartment(db);
        var employee = new Employee(Guid.NewGuid(), "Old", "Name", "old@example.com", dept.Id);
        db.Employees.Add(employee);
        await db.SaveChangesAsync();

        employee.Deactivate();
        await repo.UpdateAsync(employee);

        var result = await db.Employees.FindAsync(employee.Id);
        Assert.NotNull(result);
        Assert.False(result!.IsActive);
    }
}
