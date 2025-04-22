using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;

namespace ProjectTest.Application.Tarefas.Commands
{
    public class RemoverTarefaCommand : ICommand<bool>
    {
        public RemoverTarefaDto Dados { get; set; }

        public RemoverTarefaCommand(RemoverTarefaDto dados)
        {
            Dados = dados;
        }
    }
}
