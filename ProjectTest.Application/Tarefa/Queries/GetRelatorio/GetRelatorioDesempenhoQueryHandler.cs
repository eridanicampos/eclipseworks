using MediatR;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Interfaces;

namespace ProjectTest.Application.Tarefas.Queries
{
    public class GetRelatorioDesempenhoQueryHandler : IRequestHandler<GetRelatorioDesempenhoQuery, List<RelatorioDesempenhoDto>>
    {
        private readonly ITarefaService _tarefaService;
        private readonly ICurrentUserInfo _currentUser;

        public GetRelatorioDesempenhoQueryHandler(ITarefaService tarefaService, ICurrentUserInfo currentUser)
        {
            _tarefaService = tarefaService;
            _currentUser = currentUser;
        }

        public async Task<List<RelatorioDesempenhoDto>> Handle(GetRelatorioDesempenhoQuery request, CancellationToken cancellationToken)
        {
            return await _tarefaService.GetRelatorioDesempenhoAsync();
        }
    }
}
