using FluentValidation;
using task.Src.Application.Auth.Commands;

namespace task.Src.Application.Auth.Validators;

public class  PreRegisterCommandValidator : AbstractValidator<PreRegisterCommand>
{
    public PreRegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail fornecido não é válido.");
    }
}