using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Infrastructure.Repositories;
using PayrollApprovalSystem.Infrastructure.Tests;

public class PayrollStructureRepositoryTests
{
    private static (PayrollStructureRepository repo, AppDbContext db) Create()
    {
        var db = DbContextFactory.Create();
        return (new PayrollStructureRepository(db), db);
    }

    private static Employee SeedEmployee(AppDbContext db)
    {
        var dept = new Department(Guid.NewGuid(), "Engineering");
        db.Departments.Add(dept);
        var emp = new Employee(Guid.NewGuid(), "Test", "User", "test@example.com", dept.Id);
        db.Employees.Add(emp);
        db.SaveChanges();
        return emp;
    }

    [Fact]
    public async Task AddAsync_PersistsPayrollStructure()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        var structure = new PayrollStructure(Guid.NewGuid(), emp.Id, 5000m, 500m, 200m);

        await repo.AddAsync(structure);

        var result = await db.PayrollStructures.FindAsync(structure.Id);
        Assert.NotNull(result);
        Assert.Equal(5000m, result!.BaseSalary);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetActiveByEmployeeIdAsync_ReturnsActiveStructure()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        var structure = new PayrollStructure(Guid.NewGuid(), emp.Id, 5000m, 500m, 200m);
        db.PayrollStructures.Add(structure);
        await db.SaveChangesAsync();

        var result = await repo.GetActiveByEmployeeIdAsync(emp.Id);

        Assert.NotNull(result);
        Assert.Equal(5000m, result!.BaseSalary);
    }

    [Fact]
    public async Task GetActiveByEmployeeIdAsync_ReturnsNull_WhenDeactivated()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        var structure = new PayrollStructure(Guid.NewGuid(), emp.Id, 5000m, 500m, 200m);
        structure.Deactivate();
        db.PayrollStructures.Add(structure);
        await db.SaveChangesAsync();

        var result = await repo.GetActiveByEmployeeIdAsync(emp.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task HasActiveStructureAsync_ReturnsTrue_WhenActive()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        db.PayrollStructures.Add(new PayrollStructure(Guid.NewGuid(), emp.Id, 5000m, 500m, 200m));
        await db.SaveChangesAsync();

        var result = await repo.HasActiveStructureAsync(emp.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task HasActiveStructureAsync_ReturnsFalse_WhenNone()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);

        var result = await repo.HasActiveStructureAsync(emp.Id);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_DeactivatesStructure()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        var structure = new PayrollStructure(Guid.NewGuid(), emp.Id, 5000m, 500m, 200m);
        db.PayrollStructures.Add(structure);
        await db.SaveChangesAsync();

        structure.Deactivate();
        await repo.UpdateAsync(structure);

        var result = await db.PayrollStructures.FindAsync(structure.Id);
        Assert.NotNull(result);
        Assert.False(result!.IsActive);
    }
}
