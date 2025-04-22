using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.Tarefas.Commands
{
    public class UpdateTarefaCommandHandler : ICommandHandler<UpdateTarefaCommand, TarefaDto>
    {
        private readonly ITarefaService _tarefaService;
        private readonly IMapper _mapper;

        public UpdateTarefaCommandHandler(ITarefaService tarefaService, IMapper mapper)
        {
            _tarefaService = tarefaService;
            _mapper = mapper;
        }

        public async Task<TarefaDto> Handle(UpdateTarefaCommand request, CancellationToken cancellationToken)
        {
            var tarefaEntity = _mapper.Map<Tarefa>(request.Tarefa);

            var atualizada = await _tarefaService.UpdateAsync(tarefaEntity, request.Tarefa.UsuarioId);

            return _mapper.Map<TarefaDto>(atualizada);
        }
    }
}
