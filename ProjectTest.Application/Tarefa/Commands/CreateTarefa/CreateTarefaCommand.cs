using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;

namespace ProjectTest.Application.Tarefas.Commands
{
    public class CreateTarefaCommand : ICommand<TarefaDto>
    {
        public CreateTarefaDto Tarefa { get; set; }

        public CreateTarefaCommand(CreateTarefaDto tarefa)
        {
            Tarefa = tarefa;
        }
    }
}
