using Microsoft.AspNetCore.Mvc;
using NonRelationalDatabaseGoal.Interfaces.Services;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Models.Requests;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ProjectParameters parameters) =>
        Ok(await _service.GetAsync(parameters));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id) =>
        Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest project)
    {
        await _service.CreateAsync(new Project
        {
            Id = Guid.NewGuid().ToString(),
            TeamId = project.TeamId,
            Title = project.Title,
            Type = project.Type,
            Url = project.Url
        });

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
