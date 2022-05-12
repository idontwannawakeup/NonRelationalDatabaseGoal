using Microsoft.AspNetCore.Mvc;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Services;

namespace NonRelationalDatabaseGoal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly TeamService _service;

    public TeamController(TeamService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Team team)
    {
        team.Id = Guid.NewGuid().ToString();
        await _service.CreateAsync(team);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] Team team)
    {
        await _service.UpdateAsync(team);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/members/{memberId}")]
    public async Task<IActionResult> AddMember([FromRoute] string id, [FromRoute] string memberId)
    {
        await _service.AddMemberAsync(id, memberId);
        return NoContent();
    }

    [HttpDelete("{id}/members/{memberId}")]
    public async Task<IActionResult> DeleteMember([FromRoute] string id, [FromRoute] string memberId)
    {
        await _service.DeleteMemberAsync(id, memberId);
        return NoContent();
    }
}
