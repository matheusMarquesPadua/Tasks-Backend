using FluentValidation;
using task.Src.Application.Auth.Commands;

namespace task.Src.Application.Auth.Validators;

public class AssignUserRoleCommandValidator : AbstractValidator<AssignUserRoleCommand>
{
    public AssignUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("O ID do usuário deve ser maior que 0.");
        RuleFor(x => x.OrganizationUnitId)
            .GreaterThan(0).WithMessage("O ID da organização deve ser maior que 0.");
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("O nome do papel é obrigatório.")
            .MaximumLength(50).WithMessage("O nome do papel não pode exceder 50 caracteres.");
    }
}