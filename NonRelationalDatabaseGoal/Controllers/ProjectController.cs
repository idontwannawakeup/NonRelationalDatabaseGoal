using Microsoft.AspNetCore.Mvc;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;
using NonRelationalDatabaseGoal.Services;

namespace NonRelationalDatabaseGoal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _service;

    public ProjectController(ProjectService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ProjectParameters parameters) =>
        Ok(await _service.GetAsync(parameters));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id) =>
        Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Project project)
    {
        project.Id = Guid.NewGuid().ToString();
        await _service.CreateAsync(project);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] Project project)
    {
        await _service.UpdateAsync(project);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
