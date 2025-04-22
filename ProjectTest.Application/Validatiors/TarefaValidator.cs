using FluentValidation;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Domain.Validators
{
    public class TarefaValidator : AbstractValidator<Tarefa>
    {
        public TarefaValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("O título da tarefa é obrigatório.")
                .Length(5, 100).WithMessage("O título deve ter entre 5 e 100 caracteres.");

            RuleForEach(x => x.Comentarios).SetValidator(new ComentarioValidator());
        }
    }
}
