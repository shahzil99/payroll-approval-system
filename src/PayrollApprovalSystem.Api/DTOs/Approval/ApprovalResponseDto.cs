namespace PayrollApprovalSystem.Api.DTOs.Approval;

public class ApprovalResponseDto
{
    public Guid ApprovalId { get; set; }
    public Guid PayrollId { get; set; }
    public string ApprovalStatus { get; set; } = string.Empty;
    public string PayrollStatus { get; set; } = string.Empty;
    public DateTime? ReviewedAt { get; set; }
}