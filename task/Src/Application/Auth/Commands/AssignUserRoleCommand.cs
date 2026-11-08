using MediatR;
using Microsoft.EntityFrameworkCore;
using task.Src.Domain.Entities.Organization;
using task.Src.Infrastructure.Data.Context;

namespace task.Src.Application.Auth.Commands;

public record AssignUserRoleCommand(
    int UserId,
    int OrganizationUnitId, 
    string RoleName,
    bool CanAssignTasks
 ) : IRequest<bool>;

public class AssignUserRoleCommandHandler(AppDbContext context) : IRequestHandler<AssignUserRoleCommand, bool>
{
    public async Task<bool> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userExists = await context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);

        var organizationExists = await context.OrganizationUnits.AnyAsync(o => o.Id == request.OrganizationUnitId, cancellationToken);

        var userRole = new UserUnitRole
        {
            UserId = request.UserId,
            OrganizationUnitId = request.OrganizationUnitId,
            RoleName = request.RoleName,
            CanAssignTasks = request.CanAssignTasks
        };

        context.Set<UserUnitRole>().Add(userRole);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}