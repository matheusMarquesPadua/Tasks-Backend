using MediatR;
using Microsoft.AspNetCore.Identity;
using task.Src.Domain.Entities.Identity;

namespace task.Src.Domain.Entities.Identity;

public record AssignUserRoleCommand(string Email, string RoleName) : IRequest<bool>;

public class AssignUserRoleCommandHandler : IRequestHandler<AssignUserRoleCommand, bool>
{
  private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole<int>> _roleManager;

	public AssignUserRoleCommandHandler(
		UserManager<ApplicationUser> userManager,
     RoleManager<IdentityRole<int>> roleManager)
	{ 
		_roleManager = roleManager;
		_userManager = userManager;
	}

  public async Task<bool> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
	{ 
		var user = await _userManager.FindByEmailAsync(request.Email);
		if (user == null) return false;

		if (!await _roleManager.RoleExistsAsync(request.RoleName))
		{
			await _roleManager.CreateAsync(new IdentityRole<int>(request.RoleName));
		}

		if (!await _userManager.IsInRoleAsync(user, request.RoleName))
		{
			await _userManager.AddToRoleAsync(user, request.RoleName);
		}

		return true;
	}
}