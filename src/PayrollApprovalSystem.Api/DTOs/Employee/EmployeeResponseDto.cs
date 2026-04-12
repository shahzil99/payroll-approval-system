namespace PayrollApprovalSystem.Api.DTOs.Employee;

public class EmployeeResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}