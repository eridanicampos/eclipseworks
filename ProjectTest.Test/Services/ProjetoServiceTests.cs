using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Services;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Domain.Validators;
using System.Linq.Expressions;
using Xunit;

namespace ProjectTest.Tests.Services
{
    public class ProjetoServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ILogger<IProjetoService>> _loggerMock;
        private readonly ProjetoService _service;

        public ProjetoServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<IProjetoService>>();
            _service = new ProjetoService(_uowMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProjetos()
        {
            var projetos = new List<Projeto> { new Projeto { Nome = "Projeto Teste" } };
            _uowMock.Setup(u => u.ProjetoRepository.GetAllAsyncWithChildren(It.IsAny<Expression<Func<Projeto, object>>[]>()))
                .ReturnsAsync(projetos);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenInvalid()
        {
            var projeto = new Projeto { Nome = "" }; // Nome vazio

            var act = async () => await _service.CreateAsync(projeto);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProjeto_WhenValid()
        {
            var projeto = new Projeto { Nome = "Projeto Válido" };

            _uowMock.Setup(u => u.ProjetoRepository.AddAndSaveAsync(It.IsAny<Projeto>())).ReturnsAsync((Projeto p) => p);


            var result = await _service.CreateAsync(projeto);

            result.Should().NotBeNull();
            result.Nome.Should().Be("Projeto Válido");
            _uowMock.Verify(u => u.ProjetoRepository.AddAndSaveAsync(It.IsAny<Projeto>()), Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenProjetoNotFound()
        {
            var projeto = new Projeto { Id = Guid.NewGuid(), Nome = "Projeto X" };
            _uowMock.Setup(u => u.ProjetoRepository.GetByGuidAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Projeto)null);

            var act = async () => await _service.UpdateAsync(projeto, Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>().WithMessage("Projeto não encontrado");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProjeto_WhenValid()
        {
            var id = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var projeto = new Projeto { Id = id, Nome = "Atualizado" };
            var projetoBanco = new Projeto { Id = id, Nome = "Antigo" };

            _uowMock.Setup(u => u.ProjetoRepository.GetByGuidAsync(id))
                .ReturnsAsync(projetoBanco);

            _uowMock.Setup(u => u.ProjetoRepository.UpdateAsync(It.IsAny<Projeto>()))
                .Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(projeto, usuarioId);

            result.Should().NotBeNull();
            result.Nome.Should().Be("Atualizado");
            result.UpdatedByUserId.Should().Be(usuarioId.ToString());
        }


        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenProjetoNotFound()
        {
            _uowMock.Setup(u => u.ProjetoRepository.GetByGuidAsyncWithChildren(It.IsAny<Guid>(), It.IsAny<Expression<Func<Projeto, object>>>()))
                .ReturnsAsync((Projeto)null);

            var act = async () => await _service.DeleteAsync(Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>().WithMessage("Projeto não encontrado.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenTarefasPendentes()
        {
            var projeto = new Projeto
            {
                Tarefas = new List<Tarefa>
                {
                    new Tarefa(EPrioridade.Baixa) { Status = EStatusTarefa.Pendente }
                }
            };
            _uowMock.Setup(u => u.ProjetoRepository.GetByGuidAsyncWithChildren(It.IsAny<Guid>(), It.IsAny<Expression<Func<Projeto, object>>>()))
                .ReturnsAsync(projeto);

            var act = async () => await _service.DeleteAsync(Guid.NewGuid());

            await act.Should().ThrowAsync<ValidationException>().WithMessage("O projeto não pode ser removido enquanto houver tarefas pendentes.*");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProjeto_WhenValid()
        {
            var projeto = new Projeto
            {
                Id = Guid.NewGuid(),
                Tarefas = new List<Tarefa>() // sem tarefas pendentes
            };

            _uowMock.Setup(u => u.ProjetoRepository.GetByGuidAsyncWithChildren(projeto.Id, It.IsAny<Expression<Func<Projeto, object>>[]>()))
                .ReturnsAsync(projeto);

            _uowMock.Setup(u => u.ProjetoRepository.DeleteAndSaveAsync(projeto))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(projeto.Id);

            result.Should().BeTrue();
        }


        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenIdInvalid()
        {
            var act = async () => await _service.GetByIdAsync("invalido");

            await act.Should().ThrowAsync<Exception>().WithMessage("ID inválido");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenProjetoNotFound()
        {
            _uowMock.Setup(u => u.ProjetoRepository.GetByGuidAsyncWithChildren(
                It.IsAny<Guid>(),
                It.IsAny<Expression<Func<Projeto, object>>[]>()))
                .ReturnsAsync((Projeto)null);

            var act = async () => await _service.GetByIdAsync(Guid.NewGuid().ToString());

            await act.Should().ThrowAsync<Exception>().WithMessage("Projeto não encontrado");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProjeto_WhenExists()
        {
            var id = Guid.NewGuid();
            var projeto = new Projeto { Id = id, Nome = "Projeto Encontrado" };

            _uowMock.Setup(u => u.ProjetoRepository.GetByGuidAsyncWithChildren(id, It.IsAny<Expression<Func<Projeto, object>>[]>()))
                .ReturnsAsync(projeto);

            var result = await _service.GetByIdAsync(id.ToString());

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Nome.Should().Be("Projeto Encontrado");
        }

    }
}
