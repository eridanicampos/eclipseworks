using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Projetos.Commands.DeleteProjeto;
using ProjectTest.Application.Projetos.Commands.UpdateProjeto;
using ProjectTest.Application.Projetos.Commands;
using ProjectTest.Application.Projetos.Queries;
using ProjectTest.Domain.Helpers;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ProjectTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ProjetoController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProjetoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProjetoCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warning("Dados inválidos para criação de projeto.");
                    return BadRequest(ModelState);
                }

                _logger.Information("Criando novo projeto...");
                var projeto = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = projeto.Id }, projeto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao criar projeto.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar projeto.");
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ProjetoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProjetoCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warning("Dados inválidos para atualização de projeto.");
                    return BadRequest(ModelState);
                }

                _logger.Information("Atualizando projeto...");
                var projeto = await _mediator.Send(command);
                return Ok(projeto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao atualizar projeto.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar projeto.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                _logger.Information("Excluindo projeto. ID: {ProjetoId}", id);
                var command = new DeleteProjetoCommand(id); 
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao excluir projeto.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir projeto.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProjetoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                _logger.Information("Listando todos os projetos...");
                var query = new GetAllProjetosQuery();
                var projetos = await _mediator.Send(query);
                return Ok(projetos);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao listar projetos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao listar projetos.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjetoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            try
            {
                _logger.Information("Buscando projeto por ID: {ProjetoId}", id);
                var query = new GetProjetoByIdQuery(id);
                var projeto = await _mediator.Send(query);
                return Ok(projeto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao buscar projeto.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar projeto.");
            }
        }
    }
}
