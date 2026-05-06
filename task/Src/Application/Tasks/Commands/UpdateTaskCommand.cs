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

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
{
    private readonly AppDbContext _context;
    public UpdateTaskCommandHandler(AppDbContext context) => _context = context;

    public async System.Threading.Tasks.Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.TaskItems.FindAsync(new object[] { request.id }, cancellationToken);
        if (task == null) return false;

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.EndDate = request.EndDate;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}