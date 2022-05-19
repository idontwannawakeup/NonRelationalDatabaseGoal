using Microsoft.AspNetCore.Mvc;
using NonRelationalDatabaseGoal.Interfaces.Services;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Models.Requests;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] UserParameters parameters) =>
        Ok(await _service.GetAsync(parameters));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id) =>
        Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest user)
    {
        await _service.CreateAsync(new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Profession = user.Profession,
            Specialization = user.Specialization
        });

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] AppUser user)
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
