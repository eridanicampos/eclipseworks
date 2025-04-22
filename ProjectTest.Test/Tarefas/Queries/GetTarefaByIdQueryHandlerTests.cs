using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Tarefas.Queries;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectTest.Tests.Handlers
{
    public class GetTarefaByIdQueryHandlerTests
    {
        private readonly Mock<ITarefaService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly GetTarefaByIdQueryHandler _handler;

        public GetTarefaByIdQueryHandlerTests()
        {
            _serviceMock = new Mock<ITarefaService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProjectTest.Application.AutoMapper.AutoMapperSetup());
            });
            _mapper = config.CreateMapper();

            _handler = new GetTarefaByIdQueryHandler(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnTarefaDto_WhenIdIsValid()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            // Fix the issue by explicitly specifying the full namespace for Tarefa
            var tarefa = new ProjectTest.Domain.Entities.Tarefa(EPrioridade.Alta)
            {
                Id = tarefaId,
                Titulo = "Teste",
                Descricao = "Descricao",
                Status = EStatusTarefa.Pendente,
                DataVencimento = DateTime.UtcNow.AddDays(2),
                Comentarios = new List<Comentario>()
            };

            _serviceMock.Setup(s => s.GetById(tarefaId.ToString())).ReturnsAsync(tarefa);

            var query = new GetTarefaByIdQuery(tarefaId.ToString());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(tarefaId);
            result.Titulo.Should().Be("Teste");
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenIdIsInvalid()
        {
            // Arrange
            var query = new GetTarefaByIdQuery("id-invalido");

            _serviceMock.Setup(s => s.GetById("id-invalido")).ThrowsAsync(new Exception("ID é inválido"));

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("ID é inválido");
        }
    }
}
