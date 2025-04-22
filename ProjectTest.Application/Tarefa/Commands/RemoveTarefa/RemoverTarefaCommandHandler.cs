using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.Interfaces;

namespace ProjectTest.Application.Tarefas.Commands
{
    public class RemoverTarefaCommandHandler : ICommandHandler<RemoverTarefaCommand, bool>
    {
        private readonly ITarefaService _tarefaService;

        public RemoverTarefaCommandHandler(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        public async Task<bool> Handle(RemoverTarefaCommand request, CancellationToken cancellationToken)
        {
            return await _tarefaService.RemoverTarefaDoProjetoAsync(
                request.Dados.TarefaId,
                request.Dados.ProjetoId,
                request.Dados.UsuarioId
            );
        }
    }
}
