namespace PayrollApprovalSystem.Domain.Entities;

public class PayrollStructure
{
    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public decimal BaseSalary { get; private set; }
    public decimal Bonus { get; private set; }
    public decimal Deductions { get; private set; }
    public bool IsActive { get; private set; }

    public PayrollStructure(Guid id, Guid employeeId, decimal baseSalary, decimal bonus, decimal deductions)
    {
        Id = id;
        EmployeeId = employeeId;
        BaseSalary = baseSalary;
        Bonus = bonus;
        Deductions = deductions;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
