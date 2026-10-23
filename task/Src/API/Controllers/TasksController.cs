using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task.Src.Application.Tasks.Commands;
using task.Src.Application.Tasks.Queries;

namespace task.Src.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        var taskId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = taskId }, new {Id = taskId, Message = "Tarefa criada com sucesso!"});
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetAllTasksQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await mediator.Send(new GetTaskByIdQuery(id));
        if (result == null)
            return NotFound(new { Message = "Tarefa não encontrada." });
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskCommand command)
    {
        if (id != command.id) return BadRequest(new { Message = "ID da tarefa não corresponde ao ID do comando." });

        var success = await mediator.Send(command);
        if (!success) return NotFound(new { Message = "Tarefa não encontrada." });

        return Ok(new { Message = "Tarefa atualizada com sucesso!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var success = await mediator.Send(new DeleteTaskCommand(id));
        if (!success) return NotFound(new { Message = "Tarefa não encontrada." });

        return Ok(new { Message = "Tarefa deletada com sucesso!" });
    }
}