using FluentAssertions;
using Moq;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Tarefas.Commands;
using ProjectTest.Application.Tarefas.Commands.DeleteTarefa;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectTest.Tests.Handlers
{
    public class DeleteTarefaCommandHandlerTests
    {
        private readonly Mock<ITarefaService> _serviceMock;
        private readonly DeleteTarefaCommandHandler _handler;

        public DeleteTarefaCommandHandlerTests()
        {
            _serviceMock = new Mock<ITarefaService>();
            _handler = new DeleteTarefaCommandHandler(_serviceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenTarefaIsDeleted()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(tarefaId)).ReturnsAsync(true);

            var command = new DeleteTarefaCommand(tarefaId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenTarefaNotFoundOrFails()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(tarefaId)).ReturnsAsync(false);

            var command = new DeleteTarefaCommand(tarefaId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
