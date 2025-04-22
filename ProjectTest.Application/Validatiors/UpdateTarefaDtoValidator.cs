using FluentValidation;
using ProjectTest.Application.DTO;

namespace ProjectTest.Application.Validators
{
    public class UpdateTarefaDtoValidator : AbstractValidator<UpdateTarefaDto>
    {
        public UpdateTarefaDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID da tarefa é obrigatório.");

            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("O título é obrigatório.")
                .Length(5, 100).WithMessage("O título deve ter entre 5 e 100 caracteres.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.");

            RuleFor(x => x.DataVencimento)
                .GreaterThan(DateTime.MinValue).WithMessage("A data de vencimento deve ser válida.");

            RuleFor(x => x.UsuarioId)
                .NotEmpty().WithMessage("Usuário responsável pela alteração é obrigatório.");
        }
    }
}
