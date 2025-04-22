using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO.Projetos;
using ProjectTest.Application.DTO;

namespace ProjectTest.Application.Projetos.Commands
{
    public class CreateProjetoCommand : ICommand<ProjetoDto>
    {
        public CreateProjetoDto Projeto { get; set; }

        public CreateProjetoCommand(CreateProjetoDto projeto)
        {
            Projeto = projeto;
        }
    }
}
