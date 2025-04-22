using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;

namespace ProjectTest.Application.Tarefas.Queries
{
    public class GetAllTarefasQueryHandler : IQueryHandler<GetAllTarefasQuery, List<TarefaDto>>
    {
        private readonly ITarefaService _tarefaService;
        private readonly IMapper _mapper;

        public GetAllTarefasQueryHandler(ITarefaService tarefaService, IMapper mapper)
        {
            _tarefaService = tarefaService;
            _mapper = mapper;
        }

        public async Task<List<TarefaDto>> Handle(GetAllTarefasQuery request, CancellationToken cancellationToken)
        {
            var tarefas = await _tarefaService.GetAllAsync();
            return _mapper.Map<List<TarefaDto>>(tarefas);
        }
    }
}
