using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollApprovalSystem.Api.DTOs.Payslip;
using PayrollApprovalSystem.Api.Mappings;
using PayrollApprovalSystem.Application.Services;
using PayrollApprovalSystem.Domain.Entities;
using PayrollApprovalSystem.Domain.Exceptions;

namespace PayrollApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayslipController : ControllerBase
{
    private readonly PayslipService _payslipService;

    public PayslipController(PayslipService payslipService)
    {
        _payslipService = payslipService;
    }

    [HttpPost("generate")]
    [Authorize(Roles = "Employee,Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<PayslipResponseDto> GeneratePayslip([FromBody] GeneratePayslipRequestDto request)
    {        try
        {
            // TODO: Replace manual Payroll creation with repository/database lookup.
            // TODO: Persist generated Payslip through Infrastructure layer.
            
            var payroll = new Payroll(
                request.PayrollId,
                Guid.NewGuid(),
                4,
                2026,
                40000,
                5000,
                2000);

            payroll.Approve();

            var payslip = _payslipService.GeneratePayslip(payroll);

            return Ok(payslip.ToDto());
        }
        catch (DomainException ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                type = "DomainError"
            });
        }
    }
}