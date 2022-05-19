using Microsoft.AspNetCore.Mvc;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Models.Requests;
using NonRelationalDatabaseGoal.Parameters;
using NonRelationalDatabaseGoal.Services;

namespace NonRelationalDatabaseGoal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly TicketService _service;

    public TicketController(TicketService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TicketParameters parameters) =>
        Ok(await _service.GetAsync(parameters));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id) =>
        Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest ticket)
    {
        await _service.CreateAsync(new Ticket
        {
            Id = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            ProjectId = ticket.ProjectId,
            ExecutorId = ticket.ExecutorId,
            Title = ticket.Title,
            Type = ticket.Type,
            Description = ticket.Description,
            Status = ticket.Status,
            Priority = ticket.Priority,
            Deadline = ticket.Deadline
        });

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] Ticket ticket)
    {
        await _service.UpdateAsync(ticket);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/assign/{executorId}")]
    public async Task<IActionResult> AssignExecutor([FromRoute] string id, string executorId)
    {
        await _service.AssignExecutorAsync(id, executorId);
        return NoContent();
    }

    [HttpDelete("{id}/executor")]
    public async Task<IActionResult> RemoveExecutor([FromRoute] string id)
    {
        await _service.RemoveExecutorAsync(id);
        return NoContent();
    }
}
