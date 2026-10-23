using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using task.Src.Domain.Entities.Identity;
using task.Src.Application.Auth.Commands;

namespace task.Src.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IMediator mediator, UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpPost("pre-register")]
    public async Task<IActionResult> PreRegister([FromBody] PreRegisterCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.Success)
            return BadRequest(new { Message = result.Error });

        return Ok(new { Message = "E-mail de confirmação enviado." });
    }

    [HttpPost("confirm-and-set-password")]
    public async Task<IActionResult> ConfirmAndSetPassword([FromBody] ConfirmAndSetPasswordCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.Success)
            return BadRequest(new { Message = result.Error });

        return Ok(new { Message = "Conta confirmada com sucesso! Você já pode fazer login." });
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignUserRoleCommand command)
    {
        var success = await mediator.Send(command);
        if (!success)
            return NotFound(new { Message = "Usuário não encontrado." });

        return Ok(new { Message = "Função atribuída com sucesso." });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound(new { Message = "Usuário não encontrado." });

        var roles = await userManager.GetRolesAsync(user);

        return Ok(new
        {
            Id = user.Id,
            Email = user.Email,
            Roles = roles
        });
    }
}