using PayrollApprovalSystem.Application.Services;
using PayrollApprovalSystem.Domain.Entities;

namespace PayrollApprovalSystem.Domain.Tests.Services;

public class PayrollCalculationServiceTests
{
    [Fact]
    public void CalculateTotal_ShouldReturnCorrectAmount()
    {
        var structure = new PayrollStructure(
            Guid.NewGuid(),
            Guid.NewGuid(),
            40000,
            5000,
            2000);

        var service = new PayrollCalculationService();

        var result = service.CalculateTotal(structure);

        Assert.Equal(43000, result);
    }
}
