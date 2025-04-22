using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Projetos.Queries;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Test.Projetos.Queries
{
    public class GetAllProjetosQueryHandlerTests
    {
        private readonly Mock<IProjetoService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly GetAllProjetosQueryHandler _handler;

        public GetAllProjetosQueryHandlerTests()
        {
            _serviceMock = new Mock<IProjetoService>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Application.AutoMapper.AutoMapperSetup());
            });
            _mapper = mapperConfig.CreateMapper();

            _handler = new GetAllProjetosQueryHandler(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfProjetoDto()
        {
            // Arrange
            var projetos = new List<Projeto>
            {
                new Projeto { Nome = "Projeto 1" },
                new Projeto { Nome = "Projeto 2" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(projetos);

            // Act
            var result = await _handler.Handle(new GetAllProjetosQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Nome.Should().Be("Projeto 1");
            result[1].Nome.Should().Be("Projeto 2");
        }
    }
}