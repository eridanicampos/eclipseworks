using FluentAssertions;
using Moq;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Tarefas.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectTest.Tests.Handlers
{
    public class RemoverTarefaCommandHandlerTests
    {
        private readonly Mock<ITarefaService> _tarefaServiceMock;
        private readonly RemoverTarefaCommandHandler _handler;

        public RemoverTarefaCommandHandlerTests()
        {
            _tarefaServiceMock = new Mock<ITarefaService>();
            _handler = new RemoverTarefaCommandHandler(_tarefaServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenTarefaIsRemoved()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            _tarefaServiceMock
                .Setup(s => s.RemoverTarefaDoProjetoAsync(tarefaId, projetoId, usuarioId))
                .ReturnsAsync(true);

            var command = new RemoverTarefaCommand(new Application.DTO.RemoverTarefaDto() { TarefaId = tarefaId, ProjetoId = projetoId, UsuarioId = usuarioId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenTarefaRemovalFails()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            _tarefaServiceMock
                .Setup(s => s.RemoverTarefaDoProjetoAsync(tarefaId, projetoId, usuarioId))
                .ReturnsAsync(false);

            var command = new RemoverTarefaCommand(new Application.DTO.RemoverTarefaDto() { TarefaId = tarefaId, ProjetoId = projetoId, UsuarioId = usuarioId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
