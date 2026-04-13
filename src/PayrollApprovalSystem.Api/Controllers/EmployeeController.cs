using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollApprovalSystem.Api.DTOs.Employee;

namespace PayrollApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    // TODO: Inject IEmployeeRepository when Infrastructure layer is ready

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetEmployeeById(Guid id)
    {
        // TODO: Replace with database lookup
        return NotFound(new
        {
            message = "Employee not found (placeholder endpoint)",
            type = "NotImplemented"
        });
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllEmployees()
    {
        // TODO: Replace with database lookup
        return Ok(new
        {
            message = "List of employees (placeholder endpoint)",
            type = "NotImplemented"
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateEmployee([FromBody] CreateEmployeeRequestDto request)    {
        // TODO: Implement create employee with repository
        return StatusCode(StatusCodes.Status501NotImplemented, new
        {
            message = "Create employee not implemented yet",
            type = "NotImplemented"
        });
    }
}