using FluentValidation;
using task.Src.Application.Tasks.Commands;

namespace task.Src.Application.Tasks.Validators;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O Titulo da tarefa e obrigatorio.")
            .MaximumLength(150).WithMessage("O Titulo da tarefa nao pode exceder 150 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A Descricao da tarefa e obrigatoria.")
            .MaximumLength(500).WithMessage("A Descricao da tarefa nao pode exceder 500 caracteres.");

        RuleFor(x => x.OrganizationId)
            .GreaterThan(0).WithMessage("E obrigatorio informar a organizacao.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("A data de termino nao pode ser menor que a data de inicio.");
    }
}