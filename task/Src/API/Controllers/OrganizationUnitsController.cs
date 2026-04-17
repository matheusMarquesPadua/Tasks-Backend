using MediatR;
using Microsoft.AspNetCore.Mvc;
using task.Src.Application.OrganizationUnits.Commands;

namespace task.Src.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationUnitsController : ControllerBase
{
   private readonly IMediator _mediator;
    public OrganizationUnitsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationUnitCommand command)
    {
        var resultId = await _mediator.Send(command);
        return Ok(new
        {
            Mesage = "Setor criado com sucesso",
            OrganizationUnitId = resultId
        });
    }
}
