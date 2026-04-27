using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Infrastructure.Repositories;
using PayrollApprovalSystem.Infrastructure.Tests;

public class PayrollRepositoryTests
{
    private static (PayrollRepository repo, AppDbContext db) Create()
    {
        var db = DbContextFactory.Create();
        return (new PayrollRepository(db), db);
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
    public async Task AddAsync_PersistsPayroll()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        var payroll = new Payroll(Guid.NewGuid(), emp.Id, 1, 2026, 5000m, 500m, 200m);

        await repo.AddAsync(payroll);

        var result = await db.Payrolls.FindAsync(payroll.Id);
        Assert.NotNull(result);
        Assert.Equal(5300m, result!.TotalAmount);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPayroll_WhenExists()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        var payroll = new Payroll(Guid.NewGuid(), emp.Id, 3, 2026, 4000m, 0m, 0m);
        db.Payrolls.Add(payroll);
        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(payroll.Id);

        Assert.NotNull(result);
        Assert.Equal(3, result!.Month);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPayrolls()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        db.Payrolls.AddRange(
            new Payroll(Guid.NewGuid(), emp.Id, 1, 2026, 3000m, 0m, 0m),
            new Payroll(Guid.NewGuid(), emp.Id, 2, 2026, 3000m, 0m, 0m));
        await db.SaveChangesAsync();

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByEmployeeIdAsync_ReturnsOnlyMatchingPayrolls()
    {
        var (repo, db) = Create();
        var emp1 = SeedEmployee(db);
        var dept2 = new Department(Guid.NewGuid(), "HR");
        db.Departments.Add(dept2);
        var emp2 = new Employee(Guid.NewGuid(), "Other", "User", "other@example.com", dept2.Id);
        db.Employees.Add(emp2);
        db.Payrolls.AddRange(
            new Payroll(Guid.NewGuid(), emp1.Id, 1, 2026, 3000m, 0m, 0m),
            new Payroll(Guid.NewGuid(), emp2.Id, 1, 2026, 4000m, 0m, 0m));
        await db.SaveChangesAsync();

        var result = await repo.GetByEmployeeIdAsync(emp1.Id);

        Assert.Single(result);
        Assert.Equal(3000m, result[0].BaseSalary);
    }

    [Fact]
    public async Task ExistsForEmployeeAndPeriodAsync_ReturnsTrue_WhenExists()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        db.Payrolls.Add(new Payroll(Guid.NewGuid(), emp.Id, 4, 2026, 3000m, 0m, 0m));
        await db.SaveChangesAsync();

        var result = await repo.ExistsForEmployeeAndPeriodAsync(emp.Id, 4, 2026);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsForEmployeeAndPeriodAsync_ReturnsFalse_WhenNotExists()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);

        var result = await repo.ExistsForEmployeeAndPeriodAsync(emp.Id, 12, 2026);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPayroll()
    {
        var (repo, db) = Create();
        var emp = SeedEmployee(db);
        var payroll = new Payroll(Guid.NewGuid(), emp.Id, 1, 2026, 3000m, 0m, 0m);
        db.Payrolls.Add(payroll);
        await db.SaveChangesAsync();

        payroll.UpdateAmounts(4000m, 500m, 100m);
        await repo.UpdateAsync(payroll);

        var result = await db.Payrolls.FindAsync(payroll.Id);
        Assert.NotNull(result);
        Assert.Equal(4400m, result!.TotalAmount);
    }
}
