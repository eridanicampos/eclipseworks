using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectTest.Application.DTO;
using ProjectTest.Application.DTO.Comentarios;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Tarefas.Commands;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectTest.Tests.Handlers
{
    public class UpdateTarefaCommandHandlerTests
    {
        private readonly Mock<ITarefaService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly UpdateTarefaCommandHandler _handler;

        public UpdateTarefaCommandHandlerTests()
        {
            _serviceMock = new Mock<ITarefaService>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProjectTest.Application.AutoMapper.AutoMapperSetup());
            });
            _mapper = mapperConfig.CreateMapper();

            _handler = new UpdateTarefaCommandHandler(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnTarefaDto_WhenUpdateIsSuccessful()
        {
            // Arrange
            var tarefa = new Tarefa(EPrioridade.Alta)
            {
                Id = Guid.NewGuid(),
                Titulo = "Atualizar Tarefa",
                Descricao = "Descricao atualizada",
                DataVencimento = DateTime.UtcNow.AddDays(3),
                Status = EStatusTarefa.Concluida,
                UsuarioId = Guid.NewGuid(),
                Comentarios = new List<Comentario>()
            };

            var command = new UpdateTarefaCommand(new UpdateTarefaDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataVencimento = tarefa.DataVencimento,
                Status = tarefa.Status,
                UsuarioId = tarefa.UsuarioId,
                ProjetoId = Guid.NewGuid(),
                Comentarios = new List<UpdateComentarioDto>()
            });

            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Tarefa>(), tarefa.UsuarioId))
                .ReturnsAsync(tarefa);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Titulo.Should().Be(tarefa.Titulo);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenServiceFails()
        {
            // Arrange
            var command = new UpdateTarefaCommand(new UpdateTarefaDto
            {
                Id = Guid.NewGuid(),
                Titulo = "Erro",
                Descricao = "Erro",
                DataVencimento = DateTime.UtcNow,
                Status = EStatusTarefa.Pendente,
                UsuarioId = Guid.NewGuid(),
                ProjetoId = Guid.NewGuid(),
                Comentarios = new List<UpdateComentarioDto>()
            });

            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Tarefa>(), It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Erro ao atualizar a tarefa"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao atualizar a tarefa");
        }
    }
}
