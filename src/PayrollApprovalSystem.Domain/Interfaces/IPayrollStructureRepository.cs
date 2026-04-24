using PayrollApprovalSystem.Domain.Entities;

namespace PayrollApprovalSystem.Domain.Interfaces;

public interface IPayrollStructureRepository
{
    Task<PayrollStructure?> GetActiveByEmployeeIdAsync(Guid employeeId);
    Task<bool> HasActiveStructureAsync(Guid employeeId);
    Task AddAsync(PayrollStructure payrollStructure);
    Task UpdateAsync(PayrollStructure payrollStructure);
}