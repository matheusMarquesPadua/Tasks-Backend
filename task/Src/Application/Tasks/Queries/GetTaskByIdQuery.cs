using MediatR;
using Microsoft.EntityFrameworkCore;
using task.Src.Application.Tasks.Dto;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.Tasks.Queries;

public record GetTaskByIdQuery(int Id) : IRequest<TaskResponse?>;

public class GetTaskByIdQueryHandler(AppDbContext context) : IRequestHandler<GetTaskByIdQuery, TaskResponse?>
{
    public async Task<TaskResponse?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await context.TaskItems
            .Where(t => t.Id == request.Id)
            .Select(t => new TaskResponse(
                t.Id,
                t.Title,
                t.Description,
                t.Status,
                t.StartDate,
                t.EndDate,
                t.OrganizationId,
                t.AssignedUserId,
                t.CreatedByUserId,
                t.CreatedAt,
                t.UpdatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
        return task;
    }
}