using MediatR;
using Microsoft.AspNetCore.Identity;
using task.Src.Domain.Entities.Identity;

namespace task.Src.Domain.Entities.Identity;

public record AssignUserRoleCommand(string Email, string RoleName) : IRequest<bool>;

public class AssignUserRoleCommandHandler (
	UserManager<ApplicationUser> userManager, 
	RoleManager<IdentityRole<int>> roleManager) 
	: IRequestHandler<AssignUserRoleCommand, bool>
{
  public async Task<bool> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
	{ 
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user == null) return false;

		if (!await roleManager.RoleExistsAsync(request.RoleName))
		{
			await roleManager.CreateAsync(new IdentityRole<int>(request.RoleName));
		}

		if (!await userManager.IsInRoleAsync(user, request.RoleName))
		{
			await userManager.AddToRoleAsync(user, request.RoleName);
		}

		return true;
	}
}