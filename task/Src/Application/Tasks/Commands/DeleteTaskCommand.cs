using MediatR;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.Tasks.Commands;

public record DeleteTaskCommand(int TaskId) : IRequest<bool>;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly AppDbContext _context;
    public DeleteTaskCommandHandler(AppDbContext context) => _context = context;

    public async System.Threading.Tasks.Task<bool> Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var task = await _context.TaskItems.FindAsync(new object[] { request.TaskId }, ct);
        if (task == null) return false;

        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}