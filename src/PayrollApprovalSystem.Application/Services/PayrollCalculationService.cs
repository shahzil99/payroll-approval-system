using PayrollApprovalSystem.Domain.Entities;

namespace PayrollApprovalSystem.Application.Services;

public class PayrollCalculationService
{
    public decimal CalculateTotal(PayrollStructure structure)
    {
        return structure.BaseSalary + structure.Bonus - structure.Deductions;
    }
}
