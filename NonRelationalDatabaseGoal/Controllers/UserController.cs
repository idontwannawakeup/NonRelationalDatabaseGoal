using Microsoft.AspNetCore.Mvc;
using NonRelationalDatabaseGoal.Services;

namespace NonRelationalDatabaseGoal.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService) => _userService = userService;

    [HttpGet]
    public IActionResult GetAll() => Ok(_userService.GetAll());
}
