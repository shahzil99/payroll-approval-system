namespace PayrollApprovalSystem.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; private set; }
    public Guid DepartmentId { get; private set; }

    public Employee(Guid id, string firstName, string lastName, string email, Guid departmentId)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DepartmentId = departmentId;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
