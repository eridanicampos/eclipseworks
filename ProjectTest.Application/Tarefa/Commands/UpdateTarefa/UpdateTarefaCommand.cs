using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;

namespace ProjectTest.Application.Tarefas.Commands
{
    public class UpdateTarefaCommand : ICommand<TarefaDto>
    {
        public UpdateTarefaDto Tarefa { get; set; }

        public UpdateTarefaCommand(UpdateTarefaDto tarefa)
        {
            Tarefa = tarefa;
        }
    }
}
