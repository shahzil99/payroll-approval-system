using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Domain.Enums;
using PayrollApprovalSystem.Infrastructure.Repositories;
using PayrollApprovalSystem.Infrastructure.Tests;

public class ApprovalRepositoryTests
{
    private static (ApprovalRepository repo, AppDbContext db) Create()
    {
        var db = DbContextFactory.Create();
        return (new ApprovalRepository(db), db);
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
    public async Task AddAsync_PersistsApproval()
    {
        var (repo, db) = Create();
        var payroll = SeedPayroll(db);
        var approval = new Approval(Guid.NewGuid(), payroll.Id);

        await repo.AddAsync(approval);

        var result = await db.Approvals.FindAsync(approval.Id);
        Assert.NotNull(result);
        Assert.Equal(payroll.Id, result!.PayrollId);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsApproval_WhenExists()
    {
        var (repo, db) = Create();
        var payroll = SeedPayroll(db);
        var approval = new Approval(Guid.NewGuid(), payroll.Id);
        db.Approvals.Add(approval);
        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(approval.Id);

        Assert.NotNull(result);
        Assert.Equal(payroll.Id, result!.PayrollId);
    }

    [Fact]
    public async Task GetByPayrollIdAsync_ReturnsApproval_WhenExists()
    {
        var (repo, db) = Create();
        var payroll = SeedPayroll(db);
        var approval = new Approval(Guid.NewGuid(), payroll.Id);
        db.Approvals.Add(approval);
        await db.SaveChangesAsync();

        var result = await repo.GetByPayrollIdAsync(payroll.Id);

        Assert.NotNull(result);
        Assert.Equal(approval.Id, result!.Id);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesApproval()
    {
        var (repo, db) = Create();
        var payroll = SeedPayroll(db);
        var approval = new Approval(Guid.NewGuid(), payroll.Id);
        db.Approvals.Add(approval);
        await db.SaveChangesAsync();

        approval.Approve();
        await repo.UpdateAsync(approval);

        var result = await db.Approvals.FindAsync(approval.Id);
        Assert.NotNull(result);
        Assert.Equal(ApprovalStatus.Approved, result!.Status);
        Assert.NotNull(result.ReviewedAt);
    }
}
