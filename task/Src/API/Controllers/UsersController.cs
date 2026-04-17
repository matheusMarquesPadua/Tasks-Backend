using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; 
using task.Src.Domain.Entities.Identity; 

namespace task.Src.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(IMediator mediator, UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignUserRoleCommand command)
    {
        var success = await _mediator.Send(command);
        if (!success)
            return NotFound(new { Message = "Usuário não encontrado." });

        return Ok(new { Message = "Função atribuída com sucesso." });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null) return NotFound(new { Message = "Usuário não encontrado." });

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            Id = user.Id,
            Email = user.Email,
            Roles = roles
        });
    }
}
