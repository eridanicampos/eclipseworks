using ProjectTest.Application.Abstractions.Messaging;

namespace ProjectTest.Application.Tarefas.Commands
{
    public class DeleteTarefaCommand : ICommand<bool>
    {
        public Guid TarefaId { get; }

        public DeleteTarefaCommand(Guid tarefaId)
        {
            TarefaId = tarefaId;
        }
    }
}
