using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Tarefas.Commands;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;

namespace ProjectTest.Tests.Handlers
{
    public class CreateTarefaCommandHandlerTests
    {
        private readonly Mock<ITarefaService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly CreateTarefaCommandHandler _handler;

        public CreateTarefaCommandHandlerTests()
        {
            _serviceMock = new Mock<ITarefaService>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProjectTest.Application.AutoMapper.AutoMapperSetup());
            });
            _mapper = mapperConfig.CreateMapper();

            _handler = new CreateTarefaCommandHandler(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnTarefaDto_WhenTarefaIsCreated()
        {
            // Arrange
            var tarefa = new Tarefa(EPrioridade.Alta)
            {
                Id = Guid.NewGuid(),
                Titulo = "Tarefa Teste",
                Descricao = "Descrição teste",
                DataVencimento = DateTime.UtcNow.AddDays(5),
                Status = EStatusTarefa.Pendente,
                Comentarios = new List<Comentario>()
            };

            var command = new CreateTarefaCommand(new CreateTarefaDto
            {
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataVencimento = tarefa.DataVencimento,
                Prioridade = tarefa.Prioridade,
                Comentarios = new List<CreateComentarioDto>()
            });

            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<Tarefa>())).ReturnsAsync(tarefa);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Titulo.Should().Be(tarefa.Titulo);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCreationFails()
        {
            // Arrange
            var command = new CreateTarefaCommand(new CreateTarefaDto
            {
                Titulo = "Falha",
                Descricao = "Erro",
                DataVencimento = DateTime.UtcNow,
                Prioridade = EPrioridade.Alta
            });

            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<Tarefa>())).ReturnsAsync((Tarefa)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao criar a tarefa.");
        }
    }
}
