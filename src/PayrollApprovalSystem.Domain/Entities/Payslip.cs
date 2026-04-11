namespace PayrollApprovalSystem.Domain.Entities;

public class Payslip
{
    public Guid Id { get; private set; }
    public Guid PayrollId { get; private set; }
    public DateTime GeneratedAt { get; private set; }

    public Payslip(Guid id, Guid payrollId)
    {
        Id = id;
        PayrollId = payrollId;
        GeneratedAt = DateTime.UtcNow;
    }
}
