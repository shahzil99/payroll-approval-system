using PayrollApprovalSystem.Domain.Enums;
using PayrollApprovalSystem.Domain.Exceptions;

namespace PayrollApprovalSystem.Domain.Entities;

public class Payroll
{
    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public int Month { get; private set; }
    public int Year { get; private set; }
    public decimal BaseSalary { get; private set; }
    public decimal Bonus { get; private set; }
    public decimal Deductions { get; private set; }
    public decimal TotalAmount { get; private set; }
    public PayrollStatus Status { get; private set; }

    public Payroll(
        Guid id,
        Guid employeeId,
        int month,
        int year,
        decimal baseSalary,
        decimal bonus,
        decimal deductions)
    {
        Id = id;
        EmployeeId = employeeId;
        Month = month;
        Year = year;
        BaseSalary = baseSalary;
        Bonus = bonus;
        Deductions = deductions;
        TotalAmount = baseSalary + bonus - deductions;
        Status = PayrollStatus.Draft;
    }

    public void UpdateAmounts(decimal baseSalary, decimal bonus, decimal deductions)
    {
        if (Status == PayrollStatus.Approved)
            throw new DomainException("Approved payroll cannot be modified.");

        BaseSalary = baseSalary;
        Bonus = bonus;
        Deductions = deductions;
        TotalAmount = baseSalary + bonus - deductions;
    }

    public void Approve()
    {
        Status = PayrollStatus.Approved;
    }
}
