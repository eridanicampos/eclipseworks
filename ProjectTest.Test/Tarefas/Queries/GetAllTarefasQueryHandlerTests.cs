using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Tarefas.Queries;
using ProjectTest.Domain.Entities.Enum;
using ProjectTest.Domain.Entities;
namespace ProjectTest.Tests.Handlers
{
    public class GetAllTarefasQueryHandlerTests
    {
        private readonly Mock<ITarefaService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly GetAllTarefasQueryHandler _handler;

        public GetAllTarefasQueryHandlerTests()
        {
            _serviceMock = new Mock<ITarefaService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProjectTest.Application.AutoMapper.AutoMapperSetup());
            });
            _mapper = config.CreateMapper();

            _handler = new GetAllTarefasQueryHandler(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedTarefas()
        {
            // Arrange
            var tarefas = new List<Tarefa>
            {
                new Tarefa(EPrioridade.Media)
                {
                    Id = Guid.NewGuid(),
                    Titulo = "Tarefa Teste",
                    Descricao = "Descricao",
                    Status = EStatusTarefa.Pendente,
                    DataVencimento = DateTime.UtcNow.AddDays(2)
                }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(tarefas);

            // Act
            var result = await _handler.Handle(new GetAllTarefasQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Titulo.Should().Be("Tarefa Teste");
        }
    }
}
