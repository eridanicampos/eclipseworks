using MediatR;
using ProjectTest.Application.DTO;

namespace ProjectTest.Application.Tarefas.Queries
{
    public class GetRelatorioDesempenhoQuery : IRequest<List<RelatorioDesempenhoDto>>
    {
    }
}
