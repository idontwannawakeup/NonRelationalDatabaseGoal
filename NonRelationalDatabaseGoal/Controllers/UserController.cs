using Microsoft.AspNetCore.Mvc;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;
using NonRelationalDatabaseGoal.Services;

namespace NonRelationalDatabaseGoal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryStringParameters parameters) =>
        Ok(await _service.GetAsync(parameters));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id) =>
        Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        user.Id = Guid.NewGuid().ToString();
        await _service.CreateAsync(user);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] User user)
    {
        await _service.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
