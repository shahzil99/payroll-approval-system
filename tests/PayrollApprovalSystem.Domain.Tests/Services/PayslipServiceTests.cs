using PayrollApprovalSystem.Application.Services;
using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Domain.Exceptions;

namespace PayrollApprovalSystem.Domain.Tests.Services;

public class PayslipServiceTests
{
    [Fact]
    public void GeneratePayslip_ShouldThrow_WhenPayrollIsNotApproved()
    {
        var payroll = new Payroll(Guid.NewGuid(), Guid.NewGuid(), 1, 2026, 40000, 5000, 1000);
        var service = new PayslipService();

        Assert.Throws<DomainException>(() => service.GeneratePayslip(payroll));
    }

    [Fact]
    public void GeneratePayslip_ShouldSucceed_WhenPayrollIsApproved()
    {
        var payroll = new Payroll(Guid.NewGuid(), Guid.NewGuid(), 1, 2026, 40000, 5000, 1000);
        var approval = new Approval(Guid.NewGuid(), payroll.Id);
        var approvalService = new ApprovalService();
        var payslipService = new PayslipService();

        approvalService.ApprovePayroll(payroll, approval);

        var payslip = payslipService.GeneratePayslip(payroll);

        Assert.NotNull(payslip);
        Assert.Equal(payroll.Id, payslip.PayrollId);
    }
}
