using AutoMapper;
using Microsoft.Extensions.Logging;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Domain.Validators;
using System.ComponentModel.DataAnnotations;

namespace ProjectTest.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICurrentUserInfo _currentUser;
        private readonly ILogger<TarefaService> _logger;

        public TarefaService(IUnitOfWork uow, IMapper mapper, ICurrentUserInfo currentUser, ILogger<TarefaService> logger)
        {
            this._uow = uow;
            this._mapper = mapper;
            this._currentUser = currentUser;
            this._logger = logger;
        }

        public async Task<List<Tarefa>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Buscando todas as tarefas...");
                var _entities = await _uow.TarefaRepository.GetAllAsyncWithChildren(x => x.Projeto, x => x.Comentarios, x => x.Usuario);
                _logger.LogInformation($"Total de {_entities.Count} tarefas encontradas.");
                return _entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as tarefas");
                throw;
            }
        }

        public async Task<List<Tarefa>> GetTarefaByProjetoIdAsync(Guid projetoId)
        {
            try
            {
                _logger.LogInformation("Buscando todas as tarefas do Projeto - " + projetoId.ToString());
                var _entities = await _uow.TarefaRepository.GetTarefaByProjetoIdAsync(projetoId);
                _logger.LogInformation($"Total de {_entities.Count} tarefas encontradas.");
                return _entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as tarefas");
                throw;
            }
        }

        public async Task<Tarefa> CreateAsync(Tarefa tarefa)
        {
            try
            {
                _logger.LogInformation("Criando uma nova tarefa...");
                var validator = new TarefaValidator();
                var validationResult = await validator.ValidateAsync(tarefa);

                if (!validationResult.IsValid)
                {
                    var errorMessages = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    throw new ValidationException(errorMessages);
                }

                await _uow.TarefaRepository.AddAndSaveAsync(tarefa);

                _logger.LogInformation($"Tarefa criada com sucesso. ID: {tarefa.Id}");
                return tarefa;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar a tarefa");
                throw;
            }
        }
        public async Task<Tarefa> UpdateAsync(Tarefa tarefaEditada, Guid usuarioId)
        {
            var tarefaAtual = await _uow.TarefaRepository
                .GetByGuidAsyncWithChildren(tarefaEditada.Id, t => t.Comentarios);

            if (tarefaAtual == null)
                throw new Exception("Tarefa não encontrada!");

            _logger.LogInformation($"Atualizando a tarefa com ID: {tarefaEditada.Id}");


            var validator = new TarefaValidator();
            var validation = await validator.ValidateAsync(tarefaEditada);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var camposAlterados = new List<string>();
            if (tarefaAtual.Titulo != tarefaEditada.Titulo) camposAlterados.Add(nameof(tarefaAtual.Titulo));
            if (tarefaAtual.Descricao != tarefaEditada.Descricao) camposAlterados.Add(nameof(tarefaAtual.Descricao));
            if (tarefaAtual.Status != tarefaEditada.Status) camposAlterados.Add(nameof(tarefaAtual.Status));
            if (tarefaAtual.DataVencimento != tarefaEditada.DataVencimento) camposAlterados.Add(nameof(tarefaAtual.DataVencimento));

            var comentariosAtuais = tarefaAtual.Comentarios?.Select(c => c.Texto).ToList() ?? new();
            var comentariosEditados = tarefaEditada.Comentarios?.Select(c => c.Texto).ToList() ?? new();

            var adicionados = comentariosEditados.Except(comentariosAtuais).ToList();
            var removidos = comentariosAtuais.Except(comentariosEditados).ToList();

            if (adicionados.Any()) camposAlterados.Add("ComentarioAdicionado");
            if (removidos.Any()) camposAlterados.Add("ComentarioRemovido");

            var historico = new HistoricoAlteracao
            {
                TarefaId = tarefaAtual.Id,
                UsuarioId = usuarioId,
                Tipo = ETipoAllteracao.Alteracao,
                CamposAlterados = string.Join(",", camposAlterados),
                JsonTarefaAntesAlterada = System.Text.Json.JsonSerializer.Serialize(new
                {
                    tarefaAtual.Titulo,
                    tarefaAtual.Descricao,
                    tarefaAtual.Status,
                    tarefaAtual.DataVencimento,
                    Comentarios = comentariosAtuais
                }),
                JsonTarefaDepoisAlterada = System.Text.Json.JsonSerializer.Serialize(new
                {
                    tarefaEditada.Titulo,
                    tarefaEditada.Descricao,
                    tarefaEditada.Status,
                    tarefaEditada.DataVencimento,
                    Comentarios = comentariosEditados
                }),
                DataAlteracao = DateTime.UtcNow
            };

            tarefaAtual.Titulo = tarefaEditada.Titulo;
            tarefaAtual.Descricao = tarefaEditada.Descricao;
            tarefaAtual.DataVencimento = tarefaEditada.DataVencimento;
            tarefaAtual.Status = tarefaEditada.Status;
            tarefaAtual.UsuarioId = tarefaEditada.UsuarioId;

            tarefaAtual.Comentarios = tarefaEditada.Comentarios;

            await _uow.TarefaRepository.UpdateNotSaveAsync(tarefaAtual);
            await _uow.HistoricoAlteracaoRepository.AddAsync(historico);

            await _uow.CommitAsync();

            _logger.LogInformation($"Tarefa com ID: {tarefaEditada.Id} atualizada com sucesso.");
            return tarefaAtual;
        }


        public async Task<bool> RemoverTarefaDoProjetoAsync(Guid tarefaId, Guid projetoId, Guid usuarioId)
        {
            _logger.LogInformation($"Removendo tarefa {tarefaId} do projeto {projetoId}");

            var projeto = await _uow.ProjetoRepository.GetByGuidAsync(projetoId);
            if (projeto == null)
                throw new Exception("Projeto não encontrado");

            var tarefa = projeto.Tarefas?.FirstOrDefault(t => t.Id == tarefaId);
            if (tarefa == null)
                throw new Exception("Tarefa não encontrada no projeto");

            projeto.Tarefas.Remove(tarefa);

            var historico = new HistoricoAlteracao
            {
                TarefaId = tarefa.Id,
                Tipo = ETipoAllteracao.Exclusao,
                UsuarioId = usuarioId,
                CamposAlterados = "*",
                JsonTarefaAntesAlterada = System.Text.Json.JsonSerializer.Serialize(tarefa),
                JsonTarefaDepoisAlterada = null,
                DataAlteracao = DateTime.UtcNow
            };

            await _uow.HistoricoAlteracaoRepository.AddAndSaveAsync(historico);

            await _uow.ProjetoRepository.UpdateAsync(projeto);

            _logger.LogInformation($"Tarefa {tarefaId} removida com sucesso do projeto {projetoId}");

            return true;

        }
        public async Task<Tarefa> GetById(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                {
                    _logger.LogWarning($"ID inválido: {id}");
                    throw new Exception("ID é inválido");
                }

                _logger.LogInformation($"Buscando a tarefa com ID: {id}");
                var _entity = await _uow.TarefaRepository.GetByGuidAsyncWithChildren(guidId, x => x.Projeto, x => x.Usuario, x => x.Comentarios);
                if (_entity == null)
                {
                    _logger.LogWarning($"Tarefa não encontrada para o ID: {id}");
                    throw new Exception("Tarefa não encontrada!");
                }

                return _entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar a tarefa com ID: {id}");
                throw;
            }
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Excluindo a tarefa com ID: {id}");
                var _entity = await _uow.TarefaRepository.GetByGuidAsync(id);
                if (_entity == null)
                {
                    _logger.LogWarning($"Tarefa não encontrada para o ID: {id}");
                    throw new Exception("Tarefa não encontrada!");
                }

                await _uow.TarefaRepository.SoftDeleteAsync(id);
                _logger.LogInformation($"Tarefa com ID: {id} excluída com sucesso.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir a tarefa com ID: {id}");
                throw;
            }
        }

        public async Task<List<RelatorioDesempenhoDto>> GetRelatorioDesempenhoAsync()
        {
            var dataLimite = DateTime.UtcNow.AddDays(-30);
            var tarefas = await _uow.TarefaRepository.GetTarefasConcluidasComUsuarioAsync(dataLimite);

            var relatorio = tarefas
                .GroupBy(t => new { t.UpdatedByUserId, t.Usuario })
                .Select(g => new RelatorioDesempenhoDto
                {
                    UsuarioId = g.Key.UpdatedByUserId,
                    NomeUsuario = g.Key.Usuario?.Nome ?? "Desconhecido",
                    TotalTarefasConcluidas = g.Count()
                })
                .ToList();

            return relatorio;
        }

    }
}
