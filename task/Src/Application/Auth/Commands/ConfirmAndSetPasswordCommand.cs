using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using task.Src.Domain.Entities.Identity;

namespace task.Src.Application.Auth.Commands;

public record ConfirmAndSetPasswordCommand(
    int UserId,
    string Token,
    string Password,
    string ConfirmPassword
) : IRequest<ConfirmAndSetPasswordResult>;

public record ConfirmAndSetPasswordResult(bool Success, string? Error = null);

public class ConfirmAndSetPasswordHandler : IRequestHandler<ConfirmAndSetPasswordCommand, ConfirmAndSetPasswordResult>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmAndSetPasswordHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ConfirmAndSetPasswordResult> Handle(ConfirmAndSetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
            return new ConfirmAndSetPasswordResult(false, "As senhas não coincidem.");

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return new ConfirmAndSetPasswordResult(false, "Usuário não encontrado.");

        if (user.EmailConfirmed)
            return new ConfirmAndSetPasswordResult(false, "Conta já confirmada.");

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
        if (!confirmResult.Succeeded)
            return new ConfirmAndSetPasswordResult(false, "Token inválido ou expirado.");

        var passwordResult = await _userManager.AddPasswordAsync(user, request.Password);
        if (!passwordResult.Succeeded)
            return new ConfirmAndSetPasswordResult(false, passwordResult.Errors.First().Description);

        return new ConfirmAndSetPasswordResult(true);
    }
}