using MediatR;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.Tasks.Commands;

public record DeleteTaskCommand(int TaskId) : IRequest<bool>;

public class DeleteTaskCommandHandler(AppDbContext context) : IRequestHandler<DeleteTaskCommand, bool>
{

    public async System.Threading.Tasks.Task<bool> Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var task = await context.TaskItems.FindAsync(new object[] { request.TaskId }, ct);
        if (task == null) return false;

        context.TaskItems.Remove(task);
        await context.SaveChangesAsync(ct);
        return true;
    }
}