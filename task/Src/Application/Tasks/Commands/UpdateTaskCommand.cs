using MediatR;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.Tasks.Commands;

public record UpdateTaskCommand(
    int id,
    string Title,
    string Description,
    bool? Status,
    DateTime? EndDate
) : IRequest<bool>;

public class UpdateTaskCommandHandler(AppDbContext context) : IRequestHandler<UpdateTaskCommand, bool>
{
    public async System.Threading.Tasks.Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await context.TaskItems.FindAsync(new object[] { request.id }, cancellationToken);
        if (task == null) return false;

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.EndDate = request.EndDate;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}