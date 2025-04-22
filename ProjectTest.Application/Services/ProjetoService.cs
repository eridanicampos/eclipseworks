using Microsoft.Extensions.Logging;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Validators;
using FluentValidation;
using ProjectTest.Application.DTO;

namespace ProjectTest.Application.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<IProjetoService> _logger;

        public ProjetoService(IUnitOfWork uow, ILogger<IProjetoService> logger)
        {
            this._uow = uow;
            this._logger = logger;
        }

        public async Task<List<Projeto>> GetAllAsync()
        {
            _logger.LogInformation("Buscando todos os projetos...");
            var projetos = await _uow.ProjetoRepository.GetAllAsyncWithChildren(x => x.Tarefas, x=> x.Usuario);
            return projetos;
        }



        public async Task<Projeto> CreateAsync(Projeto projeto)
        {
            _logger.LogInformation($"Iniciando CreateAsync");

            var validator = new ProjetoValidator();
            var validation = await validator.ValidateAsync(projeto);

            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            await _uow.ProjetoRepository.AddAndSaveAsync(projeto);

            _logger.LogInformation($"Fim do CreateAsync");

            return projeto;
        }

        public async Task<Projeto> UpdateAsync(Projeto projetoEditado, Guid usuarioId)
        {
            _logger.LogInformation($"Atualizando projeto ID: {projetoEditado.Id}");

            var projetoAtual = await _uow.ProjetoRepository.GetByGuidAsync(projetoEditado.Id);
            if (projetoAtual == null)
                throw new Exception("Projeto não encontrado");

            var validator = new ProjetoValidator();
            var validation = await validator.ValidateAsync(projetoEditado);

            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            projetoAtual.Nome = projetoEditado.Nome;
            projetoAtual.UpdateAt = DateTime.UtcNow;
            projetoAtual.UpdatedByUserId = usuarioId.ToString();

            await _uow.ProjetoRepository.UpdateAsync(projetoAtual);

            _logger.LogInformation($"Projeto atualizado com sucesso: {projetoAtual.Id}");
            return projetoAtual;
        }


        public async Task<bool> DeleteAsync(Guid projetoId)
        {
            _logger.LogInformation($"Iniciando exclusão do projeto {projetoId}");

            var projeto = await _uow.ProjetoRepository.GetByGuidAsyncWithChildren(projetoId, p => p.Tarefas);
            if (projeto == null)
            {
                _logger.LogWarning($"Projeto {projetoId} não encontrado.");
                throw new Exception("Projeto não encontrado.");
            }

            if (projeto.Tarefas != null && projeto.Tarefas.Any(t => t.Status == Domain.Entities.Enum.EStatusTarefa.Pendente))
            {
                _logger.LogWarning($"Projeto {projetoId} contém tarefas pendentes. Exclusão não permitida.");
                throw new ValidationException("O projeto não pode ser removido enquanto houver tarefas pendentes. Finalize ou remova as tarefas primeiro.");
            }

            await _uow.ProjetoRepository.DeleteAndSaveAsync(projeto);

            _logger.LogInformation($"Projeto {projetoId} removido com sucesso.");
            return true;
        }

        public async Task<Projeto> GetByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                _logger.LogWarning("ID inválido");
                throw new Exception("ID inválido");
            }

            var projeto = await _uow.ProjetoRepository.GetByGuidAsyncWithChildren(guidId, x=> x.Tarefas);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado");
                throw new Exception("Projeto não encontrado");
            }

            return projeto;
        }


    }
}
