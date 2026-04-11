using PayrollApprovalSystem.Application.Services;
using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Domain.Enums;

namespace PayrollApprovalSystem.Domain.Tests.Services;

public class ApprovalServiceTests
{
    [Fact]
    public void ApprovePayroll_ShouldSetPayrollAndApprovalToApproved()
    {
        var payroll = new Payroll(Guid.NewGuid(), Guid.NewGuid(), 1, 2026, 40000, 5000, 1000);
        var approval = new Approval(Guid.NewGuid(), payroll.Id);
        var service = new ApprovalService();

        service.ApprovePayroll(payroll, approval);

        Assert.Equal(PayrollStatus.Approved, payroll.Status);
        Assert.Equal(ApprovalStatus.Approved, approval.Status);
    }
}
