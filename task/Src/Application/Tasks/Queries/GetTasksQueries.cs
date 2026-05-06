using MediatR;
using Microsoft.EntityFrameworkCore;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.Tasks.Queries;

// 1. O DTO (O contrato limpo que o Front-end vai receber)
public class TaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? Status { get; set; }
    public DateTime? EndDate { get; set; }
    public int OrganizationId { get; set; }
}

// 2. Query para buscar TODAS as tarefas
public record GetAllTasksQuery() : IRequest<List<TaskResponse>>;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, List<TaskResponse>>
{
    private readonly AppDbContext _context;
    public GetAllTasksQueryHandler(AppDbContext context) => _context = context;

    public async System.Threading.Tasks.Task<List<TaskResponse>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        return await _context.TaskItems.AsNoTracking()
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                EndDate = t.EndDate,
                OrganizationId = t.OrganizationId
            }).ToListAsync(cancellationToken);
    }
}

// 3. Query para buscar UMA tarefa específica pelo ID
public record GetTaskByIdQuery(int Id) : IRequest<TaskResponse?>;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskResponse?>
{
    private readonly AppDbContext _context;
    public GetTaskByIdQueryHandler(AppDbContext context) => _context = context;

    public async System.Threading.Tasks.Task<TaskResponse?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.TaskItems.AsNoTracking()
            .Where(t => t.Id == request.Id)
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                EndDate = t.EndDate,
                OrganizationId = t.OrganizationId
            }).FirstOrDefaultAsync(cancellationToken);
    }
}