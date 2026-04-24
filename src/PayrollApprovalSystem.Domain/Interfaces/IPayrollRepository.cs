using PayrollApprovalSystem.Domain.Entities;

namespace PayrollApprovalSystem.Domain.Interfaces;

public interface IPayrollRepository
{
    Task<Payroll?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Payroll>> GetByEmployeeIdAsync(Guid employeeId);
    Task<bool> ExistsForEmployeeAndPeriodAsync(Guid employeeId, int month, int year);
    Task AddAsync(Payroll payroll);
    Task UpdateAsync(Payroll payroll);
}