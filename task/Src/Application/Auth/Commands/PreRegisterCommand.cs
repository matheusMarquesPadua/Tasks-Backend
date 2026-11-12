using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text;
using task.Src.Domain.Entities.Identity;
using task.Src.Infrastructure.Services;

namespace task.Src.Application.Auth.Commands;

public record PreRegisterCommand(string Email) : IRequest<PreRegisterResult>;
public record PreRegisterResult(bool Success, string? Error = null);

public class PreRegisterHandler(
    UserManager<ApplicationUser> userManager,
    IEmailSender emailSender,
    IOptions<AppSettings> appSettings
) : IRequestHandler<PreRegisterCommand, PreRegisterResult>
{
    public async Task<PreRegisterResult> Handle(PreRegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return new PreRegisterResult(false, "Email já cadastrado.");

        var extractedUserName = request.Email.Split('@')[0];

        var user = new ApplicationUser
        {
            UserName = extractedUserName,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
            return new PreRegisterResult(false, result.Errors.First().Description);

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var confirmLink = $"{appSettings.Value.FrontendUrl}/confirm-account?email={request.Email}&token={encodedToken}";

        await emailSender.SendEmailAsync(
            request.Email,
            "Confirme seu cadastro",
            $@"<h2>Bem-vindo ao Task App!</h2>
            <p>Clique no link abaixo para confirmar seu e-mail e definir sua senha:</p>
            <a href='{confirmLink}'>Confirmar cadastro</a>
            <p>Atenção o link expira em 24 horas.</p>
            <p>Se você não reconhece este link, por favor ignore este e-mail.</p>"
        );

        return new PreRegisterResult(true);
    }
}