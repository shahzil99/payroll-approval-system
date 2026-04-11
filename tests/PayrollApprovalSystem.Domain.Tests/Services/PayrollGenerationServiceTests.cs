using PayrollApprovalSystem.Application.Services;
using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Domain.Exceptions;

namespace PayrollApprovalSystem.Domain.Tests.Services;

public class PayrollGenerationServiceTests
{
    [Fact]
    public void GeneratePayroll_ShouldThrow_WhenEmployeeIsInactive()
    {
        var employee = new Employee(Guid.NewGuid(), "Ali", "Khan", "ali@test.com", Guid.NewGuid());
        employee.Deactivate();

        var structure = new PayrollStructure(Guid.NewGuid(), employee.Id, 40000, 5000, 1000);
        var service = new PayrollGenerationService();

        Assert.Throws<DomainException>(() =>
            service.GeneratePayroll(employee, structure, 1, 2026, false));
    }

    [Fact]
    public void GeneratePayroll_ShouldThrow_WhenPayrollAlreadyExists()
    {
        var employee = new Employee(Guid.NewGuid(), "Ali", "Khan", "ali@test.com", Guid.NewGuid());
        var structure = new PayrollStructure(Guid.NewGuid(), employee.Id, 40000, 5000, 1000);
        var service = new PayrollGenerationService();

        Assert.Throws<DomainException>(() =>
            service.GeneratePayroll(employee, structure, 1, 2026, true));
    }

    [Fact]
    public void GeneratePayroll_ShouldCreatePayroll_WhenValid()
    {
        var employee = new Employee(Guid.NewGuid(), "Ali", "Khan", "ali@test.com", Guid.NewGuid());
        var structure = new PayrollStructure(Guid.NewGuid(), employee.Id, 40000, 5000, 1000);
        var service = new PayrollGenerationService();

        var payroll = service.GeneratePayroll(employee, structure, 1, 2026, false);

        Assert.NotNull(payroll);
        Assert.Equal(employee.Id, payroll.EmployeeId);
        Assert.Equal(44000, payroll.TotalAmount);
    }
}
