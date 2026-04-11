using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Domain.Exceptions;

namespace PayrollApprovalSystem.Application.Services;

public class PayrollGenerationService
{
    public Payroll GeneratePayroll(
        Employee employee,
        PayrollStructure structure,
        int month,
        int year,
        bool payrollAlreadyExists)
    {
        if (!employee.IsActive)
            throw new DomainException("Cannot generate payroll for inactive employee.");

        if (!structure.IsActive)
            throw new DomainException("Cannot generate payroll from inactive payroll structure.");

        if (payrollAlreadyExists)
            throw new DomainException("Payroll already exists for this employee and month.");

        return new Payroll(
            Guid.NewGuid(),
            employee.Id,
            month,
            year,
            structure.BaseSalary,
            structure.Bonus,
            structure.Deductions);
    }
}
