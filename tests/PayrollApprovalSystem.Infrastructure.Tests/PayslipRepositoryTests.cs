using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Infrastructure.Repositories;
using PayrollApprovalSystem.Infrastructure.Tests;

public class PayslipRepositoryTests
{
    private static (PayslipRepository repo, AppDbContext db) Create()
    {
        var db = DbContextFactory.Create();
        return (new PayslipRepository(db), db);
    }

    private static Payroll SeedPayroll(AppDbContext db)
    {
        var dept = new Department(Guid.NewGuid(), "Engineering");
        db.Departments.Add(dept);
        var emp = new Employee(Guid.NewGuid(), "Test", "User", "test@example.com", dept.Id);
        db.Employees.Add(emp);
        var payroll = new Payroll(Guid.NewGuid(), emp.Id, 1, 2026, 3000m, 0m, 0m);
        db.Payrolls.Add(payroll);
        db.SaveChanges();
        return payroll;
    }

    [Fact]
    public async Task AddAsync_PersistsPayslip()
    {
        var (repo, db) = Create();
        var payroll = SeedPayroll(db);
        var payslip = new Payslip(Guid.NewGuid(), payroll.Id);

        await repo.AddAsync(payslip);

        var result = await db.Payslips.FindAsync(payslip.Id);
        Assert.NotNull(result);
        Assert.Equal(payroll.Id, result!.PayrollId);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPayslip_WhenExists()
    {
        var (repo, db) = Create();
        var payroll = SeedPayroll(db);
        var payslip = new Payslip(Guid.NewGuid(), payroll.Id);
        db.Payslips.Add(payslip);
        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(payslip.Id);

        Assert.NotNull(result);
        Assert.Equal(payroll.Id, result!.PayrollId);
    }

    [Fact]
    public async Task GetByPayrollIdAsync_ReturnsPayslip_WhenExists()
    {
        var (repo, db) = Create();
        var payroll = SeedPayroll(db);
        var payslip = new Payslip(Guid.NewGuid(), payroll.Id);
        db.Payslips.Add(payslip);
        await db.SaveChangesAsync();

        var result = await repo.GetByPayrollIdAsync(payroll.Id);

        Assert.NotNull(result);
        Assert.Equal(payslip.Id, result!.Id);
    }

    [Fact]
    public async Task GetByPayrollIdAsync_ReturnsNull_WhenNotFound()
    {
        var (repo, _) = Create();

        var result = await repo.GetByPayrollIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }
}
