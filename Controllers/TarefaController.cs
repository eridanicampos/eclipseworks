using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Tarefas.Queries;
using ProjectTest.Domain.Helpers;
using ILogger = Serilog.ILogger;
using ProjectTest.Application.Tarefas.Commands;
using Microsoft.AspNetCore.Authorization;

namespace ProjectTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public TarefaController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTarefaCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warning("Dados inválidos para criação da tarefa.");
                    return BadRequest(ModelState);
                }

                _logger.Information("Criando uma nova tarefa...");
                var tarefa = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = tarefa.Id }, tarefa);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao criar a tarefa.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar a tarefa.");
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateTarefaCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warning("Dados inválidos para atualização da tarefa.");
                    return BadRequest(ModelState);
                }

                _logger.Information("Atualizando tarefa...");
                var tarefa = await _mediator.Send(command);
                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao atualizar a tarefa.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar a tarefa.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TarefaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                _logger.Information("Obtendo todas as tarefas...");
                var query = new GetAllTarefasQuery();
                var tarefas = await _mediator.Send(query);
                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao obter as tarefas.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter as tarefas.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            try
            {
                _logger.Information("Obtendo tarefa por ID: {Id}", id);
                var query = new GetTarefaByIdQuery(id);
                var tarefa = await _mediator.Send(query);
                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao obter a tarefa com ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter a tarefa.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                _logger.Information("Removendo tarefa ID: {Id}", id);
                var command = new DeleteTarefaCommand(id);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao excluir a tarefa.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir a tarefa.");
            }
        }

        [HttpGet("relatorio")]
        [Authorize(Roles = "gerente")]
        [ProducesResponseType(typeof(List<RelatorioDesempenhoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRelatorioDesempenhoAsync()
        {
            try
            {
                _logger.Information("Obtendo relatório de desempenho...");
                var query = new GetRelatorioDesempenhoQuery();
                var relatorio = await _mediator.Send(query);

                return Ok(relatorio);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao obter o relatório de desempenho.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter o relatório.");
            }
        }


    }
}
