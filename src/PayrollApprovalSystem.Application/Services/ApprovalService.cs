using PayrollApprovalSystem.Domain.Entities;

namespace PayrollApprovalSystem.Application.Services;

public class ApprovalService
{
    public void ApprovePayroll(Payroll payroll, Approval approval)
    {
        approval.Approve();
        payroll.Approve();
    }
}
