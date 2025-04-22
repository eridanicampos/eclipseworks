using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Projetos.Commands.DeleteProjeto;
using ProjectTest.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectTest.Test.Projetos.Commands
{
    public class DeleteProjetoCommandHandlerTests
    {
        private readonly Mock<IProjetoService> _projetoServiceMock;
        private readonly Mock<ILogger<DeleteProjetoCommandHandler>> _loggerMock;
        private readonly DeleteProjetoCommandHandler _handler;

        public DeleteProjetoCommandHandlerTests()
        {
            _projetoServiceMock = new Mock<IProjetoService>();
            _loggerMock = new Mock<ILogger<DeleteProjetoCommandHandler>>();
            _handler = new DeleteProjetoCommandHandler(_projetoServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenDeleteSuccessful()
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            _projetoServiceMock.Setup(s => s.DeleteAsync(projetoId)).ReturnsAsync(true);

            var command = new DeleteProjetoCommand(projetoId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            _projetoServiceMock.Setup(s => s.DeleteAsync(projetoId))
                .ThrowsAsync(new ValidationException("Projeto possui tarefas pendentes."));

            var command = new DeleteProjetoCommand(projetoId);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Projeto possui tarefas pendentes.*");
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            _projetoServiceMock.Setup(s => s.DeleteAsync(projetoId))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var command = new DeleteProjetoCommand(projetoId);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro inesperado");
        }
    }
}
