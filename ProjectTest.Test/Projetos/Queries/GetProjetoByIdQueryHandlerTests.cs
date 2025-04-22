using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Projetos.Queries;
using ProjectTest.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectTest.Test.Projetos.Queries
{
    public class GetProjetoByIdQueryHandlerTests
    {
        private readonly Mock<IProjetoService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly GetProjetoByIdQueryHandler _handler;

        public GetProjetoByIdQueryHandlerTests()
        {
            _serviceMock = new Mock<IProjetoService>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Application.AutoMapper.AutoMapperSetup());
            });
            _mapper = mapperConfig.CreateMapper();

            _handler = new GetProjetoByIdQueryHandler(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnProjetoDto_WhenProjetoExists()
        {
            // Arrange
            var projeto = new Projeto
            {
                Id = Guid.NewGuid(),
                Nome = "Projeto Existente"
            };

            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(projeto);

            var query = new GetProjetoByIdQuery(projeto.Id.ToString());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Nome.Should().Be("Projeto Existente");
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProjetoDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Projeto não encontrado"));

            var query = new GetProjetoByIdQuery(Guid.NewGuid().ToString());

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Projeto não encontrado");
        }
    }
}
