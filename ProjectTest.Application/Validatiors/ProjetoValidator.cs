using FluentValidation;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Domain.Validators
{
    public class ProjetoValidator : AbstractValidator<Projeto>
    {
        public ProjetoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do projeto deve ter no máximo 100 caracteres.");

            RuleForEach(x => x.Tarefas).SetValidator(new TarefaValidator());

            RuleFor(x => x.Tarefas)
                .Must(tarefas => tarefas == null || tarefas.Count <= 20)
                .WithMessage("O projeto não pode ter mais do que 20 tarefas.");
        }
    }
}
