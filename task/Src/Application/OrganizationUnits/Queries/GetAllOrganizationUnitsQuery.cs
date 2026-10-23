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

public class GetAllOrganizationUnitsHandler(AppDbContext context) : IRequestHandler<GetAllOrganizationUnitsQuery, List<OrganizationUnitResponse>>
{
    public async Task<List<OrganizationUnitResponse>> Handle(GetAllOrganizationUnitsQuery request, CancellationToken cancellationToken)
    {
        return await context.OrganizationUnits
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
