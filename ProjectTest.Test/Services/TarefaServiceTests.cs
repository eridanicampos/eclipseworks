using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectTest.Application.Services;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Domain.Interfaces.Common;
using System.Linq.Expressions;

namespace ProjectTest.Tests.Services
{
    public class TarefaServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICurrentUserInfo> _currentUserMock;
        private readonly Mock<ILogger<TarefaService>> _loggerMock;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _currentUserMock = new Mock<ICurrentUserInfo>();
            _loggerMock = new Mock<ILogger<TarefaService>>();

            _service = new TarefaService(
                _uowMock.Object,
                _mapperMock.Object,
                _currentUserMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnTarefas()
        {
            var tarefas = new List<Tarefa> { new Tarefa(0) { Titulo = "Teste" } };
            _uowMock.Setup(u => u.TarefaRepository.GetAllAsyncWithChildren(It.IsAny<Expression<Func<Tarefa, object>>[]>()))
                    .ReturnsAsync(tarefas);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenInvalid()
        {
            var tarefa = new Tarefa(0) { Titulo = "" }; // título inválido

            var act = async () => await _service.CreateAsync(tarefa);

            await act.Should().ThrowAsync<System.ComponentModel.DataAnnotations.ValidationException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _uowMock.Setup(u => u.TarefaRepository.GetByGuidAsync(It.IsAny<Guid>())).ReturnsAsync((Tarefa)null);

            var act = async () => await _service.DeleteAsync(Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>().WithMessage("Tarefa não encontrada!");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenTarefaNotFound()
        {
            var tarefa = new Tarefa(EPrioridade.Baixa) { Id = Guid.NewGuid() };
            _uowMock.Setup(u => u.TarefaRepository.GetByGuidAsyncWithChildren(It.IsAny<Guid>(), It.IsAny<Expression<Func<Tarefa, object>>>()))
                    .ReturnsAsync((Tarefa)null);

            var act = async () => await _service.UpdateAsync(tarefa, Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>().WithMessage("Tarefa não encontrada!");
        }

        [Fact]
        public async Task RemoverTarefaDoProjetoAsync_ShouldThrow_WhenProjetoNotFound()
        {
            _uowMock.Setup(u => u.ProjetoRepository.GetByGuidAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Projeto)null);

            var act = async () => await _service.RemoverTarefaDoProjetoAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>().WithMessage("Projeto não encontrado");
        }

        [Fact]
        public async Task RemoverTarefaDoProjetoAsync_ShouldThrow_WhenTarefaNotFound()
        {
            var projeto = new Projeto
            {
                Tarefas = new List<Tarefa>() // lista vazia
            };
            _uowMock.Setup(u => u.ProjetoRepository.GetByGuidAsync(It.IsAny<Guid>()))
                .ReturnsAsync(projeto);

            var act = async () => await _service.RemoverTarefaDoProjetoAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>().WithMessage("Tarefa não encontrada no projeto");
        }

        [Fact]
        public async Task GetById_ShouldThrow_WhenIdInvalid()
        {
            var act = async () => await _service.GetById("id-invalido");

            await act.Should().ThrowAsync<Exception>().WithMessage("ID é inválido");
        }

        [Fact]
        public async Task GetById_ShouldThrow_WhenTarefaNotFound()
        {
            var id = Guid.NewGuid().ToString();
            _uowMock.Setup(u => u.TarefaRepository.GetByGuidAsyncWithChildren(
               It.IsAny<Guid>(),
               It.IsAny<Expression<Func<Tarefa, object>>[]>()
            ));
                   

            var act = async () => await _service.GetById(id);

            await act.Should().ThrowAsync<Exception>().WithMessage("Tarefa não encontrada!");
        }

        [Fact]
        public async Task GetRelatorioDesempenhoAsync_ShouldReturnRelatorio()
        {
            var usuarioId = Guid.NewGuid().ToString();
            var tarefas = new List<Tarefa>
            {
                new Tarefa(EPrioridade.Baixa)
                {
                    UpdatedByUserId = usuarioId,
                    Usuario = new Usuario { Nome = "User Test" }
                },
                new Tarefa(EPrioridade.Baixa)
                {
                    UpdatedByUserId = usuarioId,
                    Usuario = new Usuario { Nome = "User Test" }
                }
            };

            _uowMock.Setup(u => u.TarefaRepository.GetTarefasConcluidasComUsuarioAsync(It.IsAny<DateTime>()))
                    .ReturnsAsync(tarefas);

            var result = await _service.GetRelatorioDesempenhoAsync();

            result.Should().NotBeNull();
            result.Should().ContainSingle();
            result[0].TotalTarefasConcluidas.Should().Be(2);
            result[0].NomeUsuario.Should().Be("User Test");
        }
    }
}
