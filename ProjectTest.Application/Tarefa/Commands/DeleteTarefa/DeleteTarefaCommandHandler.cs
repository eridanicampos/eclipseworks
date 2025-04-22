using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.Interfaces;

namespace ProjectTest.Application.Tarefas.Commands.DeleteTarefa
{
    public class DeleteTarefaCommandHandler : ICommandHandler<DeleteTarefaCommand, bool>
    {
        private readonly ITarefaService _tarefaService;

        public DeleteTarefaCommandHandler(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        public async Task<bool> Handle(DeleteTarefaCommand request, CancellationToken cancellationToken)
        {
            return await _tarefaService.DeleteAsync(request.TarefaId);
        }
    }
}
