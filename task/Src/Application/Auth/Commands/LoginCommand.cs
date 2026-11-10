using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using task.Src.Domain.Entities.Identity;

namespace task.Src.Application.Auth.Commands;

public record LoginCommand(
    string Email,
    string Password
    ) : IRequest<LoginResult>;

public record LoginResult(
    bool Success,
    string? Token = null,
    string? ErrorMessage = null
);

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration
) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return new LoginResult(false, ErrorMessage: "Usuário ou senha inválidos.");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            return new LoginResult(false, ErrorMessage: "Usuário ou senha inválidos.");

        if(!user.EmailConfirmed)
            return new LoginResult(false, ErrorMessage: "E-mail não confirmado. Por favor, confirme seu e-mail antes de fazer login.");

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"] ?? "UmaChaveSuperSecretaMuitoLongaParaOJWTFuncionar");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return new LoginResult(true, Token: jwtToken);
    }
}