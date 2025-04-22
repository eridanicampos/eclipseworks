using ProjectTest.Application.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using FluentValidation; 
using ProjectTest.Application.Interfaces;

namespace ProjectTest.Application.Projetos.Commands.DeleteProjeto
{
    public class DeleteProjetoCommandHandler : ICommandHandler<DeleteProjetoCommand, bool>
    {
        private readonly IProjetoService _projetoService;
        private readonly ILogger<DeleteProjetoCommandHandler> _logger;

        public DeleteProjetoCommandHandler(IProjetoService projetoService, ILogger<DeleteProjetoCommandHandler> logger)
        {
            _projetoService = projetoService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProjetoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"[Handler] Solicitada exclusão do projeto {request.ProjetoId}");
                return await _projetoService.DeleteAsync(request.ProjetoId);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning($"[Handler] Regra de negócio violada: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Handler] Erro inesperado ao excluir projeto {request.ProjetoId}");
                throw;
            }
        }
    }
}
