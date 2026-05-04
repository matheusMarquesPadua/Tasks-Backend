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

public class CreateTaskCommandHandler : IRequesteHandler<CreateTaskCommand, int>
{
    private readonly AppDbContext _context;

    public CreateTaskCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<int> Handle(CreateTaskCommand request, CancelationToken cancelationToken)
    {
        new newTask = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = null,
            StartDate = null,
            EndDate = null,
            OrganizationId = request.OrganizationId,
            AssignedUserId = request.AssignedUserId,
            CreateByUserId = request.CreatedByUserId,
        };

        _context.TaskItems.Add(newTask);

        await _context.SaveChangesAsync(cancelationToken);

        return newTask.Id;
    }
}