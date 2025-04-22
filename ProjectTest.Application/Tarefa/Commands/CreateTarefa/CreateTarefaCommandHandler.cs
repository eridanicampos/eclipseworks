using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.Tarefas.Commands
{
    public class CreateTarefaCommandHandler : ICommandHandler<CreateTarefaCommand, TarefaDto>
    {
        private readonly ITarefaService _tarefaService;
        private readonly IMapper _mapper;

        public CreateTarefaCommandHandler(ITarefaService tarefaService, IMapper mapper)
        {
            _tarefaService = tarefaService;
            _mapper = mapper;
        }

        public async Task<TarefaDto> Handle(CreateTarefaCommand request, CancellationToken cancellationToken)
        {
            var tarefaEntity = _mapper.Map<Tarefa>(request.Tarefa);

            var tarefaCriada = await _tarefaService.CreateAsync(tarefaEntity);

            if (tarefaCriada == null || tarefaCriada.Id == Guid.Empty)
                throw new Exception("Erro ao criar a tarefa.");

            return _mapper.Map<TarefaDto>(tarefaCriada);
        }
    }
}
