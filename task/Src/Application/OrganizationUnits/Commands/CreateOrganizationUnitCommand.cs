using MediatR;
using System.Threading;
using task.Src.Domain.Entities.Organization;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.OrganizationUnits.Commands;

public record CreateOrganizationUnitCommand(string Name, int? ParentUnitId) : IRequest<int>;

public class CreateOrganizationUnitCommandHandler : IRequestHandler<CreateOrganizationUnitCommand, int>
{
    private readonly AppDbContext _context;

    public CreateOrganizationUnitCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrganizationUnitCommand request, CancellationToken cancellationToken)
    {
        var newUnit = new OrganizationUnit
        {
            Name = request.Name,
            ParentUnitId = request.ParentUnitId
        };
        _context.OrganizationUnits.Add(newUnit);
        await _context.SaveChangesAsync(cancellationToken);
        return newUnit.Id;
    }

}
