using FluentValidation;
using task.Src.Application.Auth.Commands;

namespace task.Src.Application.Auth.Validators;

public class ConfirmAndSetPasswordCommandValidator : AbstractValidator<ConfirmAndSetPasswordCommand>
{
    public ConfirmAndSetPasswordCommandValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Ö e-mail fornecido não é válido.");
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("O token é obrigatório.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("As senhas não coincidem.");
    }
}