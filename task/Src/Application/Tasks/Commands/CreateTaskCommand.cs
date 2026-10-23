using MediatR;
using task.Src.Domain.Entities.Tasks;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.Tasks.Commands;

public record CreateTaskCommand(
    string Title,
    string Description,
    DateTime? StartDate,
    DateTime? EndDate,
    int OrganizationId,
    int? AssignedUserId,
    int CreatedByUserId
) : IRequest<int>;

public class CreateTaskCommandHandler(AppDbContext context) : IRequestHandler<CreateTaskCommand, int>
{
    public async System.Threading.Tasks.Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var newTask = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = null,
            StartDate = null,
            EndDate = null,
            OrganizationId = request.OrganizationId,
            AssignedUserId = request.AssignedUserId,
            CreatedByUserId = request.CreatedByUserId,
        };

        context.TaskItems.Add(newTask);

        await context.SaveChangesAsync(cancellationToken);

        return newTask.Id;
    }
}