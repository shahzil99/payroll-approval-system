using PayrollApprovalSystem.Domain.Entities;

namespace PayrollApprovalSystem.Domain.Interfaces;

public interface IPayslipRepository
{
    Task<Payslip?> GetByPayrollIdAsync(Guid payrollId);
    Task AddAsync(Payslip payslip);
}