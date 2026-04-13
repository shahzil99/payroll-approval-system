namespace PayrollApprovalSystem.Api.DTOs.Employee;

public class CreateEmployeeRequestDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}