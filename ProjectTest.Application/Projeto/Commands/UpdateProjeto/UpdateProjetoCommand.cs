using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using ProjectTest.Application.DTO.Projetos;

namespace ProjectTest.Application.Projetos.Commands.UpdateProjeto
{
    public class UpdateProjetoCommand : ICommand<ProjetoDto>
    {
        public UpdateProjetoDto Projeto { get; }

        public UpdateProjetoCommand(UpdateProjetoDto projeto)
        {
            Projeto = projeto;
        }
    }
}
