using ElevenNote.Models.Responses;
using ElevenNote.Models.User;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var RegisterResult = await _userService.RegisterUserAsync(model);
        if (RegisterResult)
        {
            TextResponse response = new("User was registered.");
            return Ok(response);
        }

        return BadRequest(new TextResponse("User could not be registered."));
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int userId)
    {
        UserDetail? detail = await _userService.GetUserByIdAsync(userId);

        if (detail is null)
            return NotFound();

        return Ok(detail);
    }
}

