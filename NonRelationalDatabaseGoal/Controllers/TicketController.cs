using Microsoft.AspNetCore.Mvc;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Services;

namespace NonRelationalDatabaseGoal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly TicketService _service;

    public TicketController(TicketService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Ticket ticket)
    {
        ticket.Id = Guid.NewGuid().ToString();
        ticket.CreatedAt = DateTime.UtcNow;
        await _service.CreateAsync(ticket);
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
}
