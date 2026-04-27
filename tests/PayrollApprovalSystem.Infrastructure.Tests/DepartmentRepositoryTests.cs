using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Infrastructure.Repositories;
using PayrollApprovalSystem.Infrastructure.Tests;

public class DepartmentRepositoryTests
{
    private static (DepartmentRepository repo, AppDbContext db) Create()
    {
        var db = DbContextFactory.Create();
        return (new DepartmentRepository(db), db);
    }

    [Fact]
    public async Task AddAsync_PersistsDepartment()
    {
        var (repo, db) = Create();
        var department = new Department(Guid.NewGuid(), "Engineering");

        await repo.AddAsync(department);

        var result = await db.Departments.FindAsync(department.Id);
        Assert.NotNull(result);
        Assert.Equal("Engineering", result!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDepartment_WhenExists()
    {
        var (repo, db) = Create();
        var department = new Department(Guid.NewGuid(), "HR");
        db.Departments.Add(department);
        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(department.Id);

        Assert.NotNull(result);
        Assert.Equal("HR", result!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        var (repo, _) = Create();

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllDepartments()
    {
        var (repo, db) = Create();
        db.Departments.AddRange(
            new Department(Guid.NewGuid(), "Engineering"),
            new Department(Guid.NewGuid(), "HR"));
        await db.SaveChangesAsync();

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count);
    }
}
