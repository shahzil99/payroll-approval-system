using PayrollApprovalSystem.Domain.Enums;

namespace PayrollApprovalSystem.Domain.Entities;

public class Approval
{
    public Guid Id { get; private set; }
    public Guid PayrollId { get; private set; }
    public ApprovalStatus Status { get; private set; }
    public DateTime? ReviewedAt { get; private set; }

    public Approval(Guid id, Guid payrollId)
    {
        Id = id;
        PayrollId = payrollId;
        Status = ApprovalStatus.Pending;
    }

    public void Approve()
    {
        Status = ApprovalStatus.Approved;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = ApprovalStatus.Rejected;
        ReviewedAt = DateTime.UtcNow;
    }
}
