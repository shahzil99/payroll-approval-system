using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollApprovalSystem.Api.DTOs.Approval;
using PayrollApprovalSystem.Api.Mappings;
using PayrollApprovalSystem.Application.Services;
using PayrollApprovalSystem.Domain.Entities;

namespace PayrollApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalController : ControllerBase
{
    private readonly ApprovalService _approvalService;

    public ApprovalController(ApprovalService approvalService)
    {
        _approvalService = approvalService;
    }

    [HttpPost("approve")]
    [Authorize(Roles = "Manager,Admin")]
    public ActionResult<ApprovalResponseDto> ApprovePayroll([FromBody] ApprovePayrollRequestDto request)
    {
        // TODO: Replace manual Payroll creation with repository/database lookup.
        var payroll = new Payroll(
            request.PayrollId,
            Guid.NewGuid(),
            4,
            2026,
            40000,
            5000,
            2000);

        // TODO: Replace manual Approval creation with repository/database lookup or persistence. 
		// TODO: Persist approval status changes through Infrastructure layer.
        var approval = new Approval(Guid.NewGuid(), payroll.Id);

        _approvalService.ApprovePayroll(payroll, approval);

        return Ok(approval.ToDto(payroll));
    }
}