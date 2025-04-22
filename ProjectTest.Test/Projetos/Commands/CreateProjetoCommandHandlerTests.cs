using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Projetos.Commands;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Test.Projetos.Commands
{
    public class CreateProjetoCommandHandlerTests
    {
        private readonly Mock<IProjetoService> _projetoServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateProjetoCommandHandler _handler;

        public CreateProjetoCommandHandlerTests()
        {
            _projetoServiceMock = new Mock<IProjetoService>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateProjetoCommandHandler(_projetoServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateProjetoAndReturnDto_WhenValid()
        {
            // Arrange
            var dto = new CreateProjetoDto { Nome = "Projeto Teste", UsuarioId = Guid.NewGuid() };
            var command = new CreateProjetoCommand(dto);
            var projeto = new Projeto { Nome = dto.Nome, UsuarioId = dto.UsuarioId };
            var projetoDto = new ProjetoDto { Nome = dto.Nome };

            _mapperMock.Setup(m => m.Map<Projeto>(dto)).Returns(projeto);
            _projetoServiceMock.Setup(s => s.CreateAsync(projeto)).ReturnsAsync(projeto);
            _mapperMock.Setup(m => m.Map<ProjetoDto>(projeto)).Returns(projetoDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Nome.Should().Be("Projeto Teste");
            _mapperMock.Verify(m => m.Map<Projeto>(dto), Times.Once);
            _mapperMock.Verify(m => m.Map<ProjetoDto>(projeto), Times.Once);
            _projetoServiceMock.Verify(s => s.CreateAsync(projeto), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenServiceThrows()
        {
            // Arrange
            var dto = new CreateProjetoDto { Nome = "Erro", UsuarioId = Guid.NewGuid() };
            var command = new CreateProjetoCommand(dto);
            var projeto = new Projeto { Nome = dto.Nome, UsuarioId = dto.UsuarioId };

            _mapperMock.Setup(m => m.Map<Projeto>(dto)).Returns(projeto);
            _projetoServiceMock.Setup(s => s.CreateAsync(projeto)).ThrowsAsync(new Exception("Erro interno"));

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro interno");
        }
    }
}
