using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;

namespace ProjectTest.Application.Tarefas.Queries
{
    public class GetTarefaByIdQueryHandler : IQueryHandler<GetTarefaByIdQuery, TarefaDto>
    {
        private readonly ITarefaService _tarefaService;
        private readonly IMapper _mapper;

        public GetTarefaByIdQueryHandler(ITarefaService tarefaService, IMapper mapper)
        {
            _tarefaService = tarefaService;
            _mapper = mapper;
        }

        public async Task<TarefaDto> Handle(GetTarefaByIdQuery request, CancellationToken cancellationToken)
        {
            var tarefa = await _tarefaService.GetById(request.TarefaId);
            return _mapper.Map<TarefaDto>(tarefa);
        }
    }
}
