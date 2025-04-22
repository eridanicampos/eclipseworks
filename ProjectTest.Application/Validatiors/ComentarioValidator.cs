using FluentValidation;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Domain.Validators
{
    public class ComentarioValidator : AbstractValidator<Comentario>
    {
        public ComentarioValidator()
        {
            RuleFor(x => x.Texto)
                .NotEmpty().WithMessage("Texto do comentário é obrigatório.")
                .MaximumLength(500).WithMessage("Comentário não pode ter mais que 500 caracteres.");
        }
    }
}
