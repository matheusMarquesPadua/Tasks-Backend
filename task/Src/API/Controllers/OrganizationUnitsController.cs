using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task.Src.Application.OrganizationUnits.Commands;
using task.Src.Application.OrganizationUnits.Queries;

namespace task.Src.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationUnitsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationUnitCommand command)
    {
        var resultId = await mediator.Send(command);
        return Ok(new
        {
            Mesage = "Setor criado com sucesso",
            OrganizationUnitId = resultId
        });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllOrganizationUnitsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
