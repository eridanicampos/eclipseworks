using FluentAssertions;
using Moq;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Tarefas.Queries;
using ProjectTest.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectTest.Tests.Handlers
{
    public class GetRelatorioDesempenhoQueryHandlerTests
    {
        private readonly Mock<ITarefaService> _tarefaServiceMock;
        private readonly Mock<ICurrentUserInfo> _currentUserMock;
        private readonly GetRelatorioDesempenhoQueryHandler _handler;

        public GetRelatorioDesempenhoQueryHandlerTests()
        {
            _tarefaServiceMock = new Mock<ITarefaService>();
            _currentUserMock = new Mock<ICurrentUserInfo>();
            _handler = new GetRelatorioDesempenhoQueryHandler(_tarefaServiceMock.Object, _currentUserMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnRelatorioDesempenhoList()
        {
            // Arrange
            var expected = new List<RelatorioDesempenhoDto>
            {
                new RelatorioDesempenhoDto
                {
                    UsuarioId = Guid.NewGuid().ToString(),
                    NomeUsuario = "João",
                    TotalTarefasConcluidas = 5
                }
            };

            _tarefaServiceMock
                .Setup(service => service.GetRelatorioDesempenhoAsync())
                .ReturnsAsync(expected);

            // Act
            var result = await _handler.Handle(new GetRelatorioDesempenhoQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoDataExists()
        {
            // Arrange
            _tarefaServiceMock
                .Setup(service => service.GetRelatorioDesempenhoAsync())
                .ReturnsAsync(new List<RelatorioDesempenhoDto>());

            // Act
            var result = await _handler.Handle(new GetRelatorioDesempenhoQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }
    }
}
