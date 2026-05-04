using MediatR;
using Microsoft.EntityFrameworkCore;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.OrganizationUnits.Queries;

public class OrganizationUnitResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentUnitId { get; set; }
}

public record GetAllOrganizationUnitsQuery : IRequest<List<OrganizationUnitResponse>>;

public class GetAllOrganizationUnitsHandler : IRequestHandler<GetAllOrganizationUnitsQuery, List<OrganizationUnitResponse>>
{
    private readonly AppDbContext _context;

    public GetAllOrganizationUnitsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrganizationUnitResponse>> Handle(GetAllOrganizationUnitsQuery request, CancellationToken cancellationToken)
    {
        return await _context.OrganizationUnits
            .AsNoTracking()
            .Select(o => new OrganizationUnitResponse
            {
                Id = o.Id,
                Name = o.Name,
                ParentUnitId = o.ParentUnitId
            })
            .ToListAsync(cancellationToken);
    }
}
