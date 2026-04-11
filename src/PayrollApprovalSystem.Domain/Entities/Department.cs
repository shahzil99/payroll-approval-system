namespace PayrollApprovalSystem.Domain.Entities;

public class Department
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public Department(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
