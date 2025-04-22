using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectTest.Application.DTO;
using ProjectTest.Application.DTO.Projetos;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Projetos.Commands.UpdateProjeto;
using ProjectTest.Domain.Entities;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectTest.Test.Projetos.Commands
{
    public class UpdateProjetoCommandHandlerTests
    {
        private readonly Mock<IProjetoService> _projetoServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateProjetoCommandHandler _handler;

        public UpdateProjetoCommandHandlerTests()
        {
            _projetoServiceMock = new Mock<IProjetoService>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateProjetoCommandHandler(_projetoServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateProjetoSuccessfully()
        {
            // Arrange
            var dto = new UpdateProjetoDto
            {
                Id = Guid.NewGuid(),
                Nome = "Projeto Atualizado",
                UsuarioId = Guid.NewGuid()
            };

            var entity = new Projeto { Id = dto.Id, Nome = dto.Nome };
            var updatedEntity = new Projeto { Id = dto.Id, Nome = dto.Nome, UpdatedByUserId = dto.UsuarioId.ToString() };
            var expectedDto = new ProjetoDto { Id = dto.Id, Nome = dto.Nome };

            _mapperMock.Setup(m => m.Map<Projeto>(dto)).Returns(entity);
            _projetoServiceMock.Setup(s => s.UpdateAsync(entity, dto.UsuarioId)).ReturnsAsync(updatedEntity);
            _mapperMock.Setup(m => m.Map<ProjetoDto>(updatedEntity)).Returns(expectedDto);

            var command = new UpdateProjetoCommand(dto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(dto.Id);
            result.Nome.Should().Be(dto.Nome);
            _projetoServiceMock.Verify(s => s.UpdateAsync(entity, dto.UsuarioId), Times.Once);
        }
    }
}
