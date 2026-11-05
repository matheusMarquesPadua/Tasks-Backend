using MediatR;
using Microsoft.EntityFrameworkCore;
using task.Src.Application.Tasks.Dto;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.Tasks.Queries;

public record GetAllTasksQuery() : IRequest<List<TaskResponse>>;

public class GetAllTasksQueryHandler(AppDbContext context) : IRequestHandler<GetAllTasksQuery, List<TaskResponse>>
{
	public async Task<List<TaskResponse>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
	{ 
		return await context.TaskItems
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
			.ToListAsync(cancellationToken);
	}
}