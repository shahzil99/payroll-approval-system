using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Domain.Enums;
using PayrollApprovalSystem.Domain.Exceptions;

namespace PayrollApprovalSystem.Application.Services;

public class PayslipService
{
    public Payslip GeneratePayslip(Payroll payroll)
    {
        if (payroll.Status != PayrollStatus.Approved)
            throw new DomainException("Payslip can only be generated for approved payroll.");

        return new Payslip(Guid.NewGuid(), payroll.Id);
    }
}
