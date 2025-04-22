using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.Projetos.Commands.UpdateProjeto
{
    public class UpdateProjetoCommandHandler : ICommandHandler<UpdateProjetoCommand, ProjetoDto>
    {
        private readonly IProjetoService _projetoService;
        private readonly IMapper _mapper;

        public UpdateProjetoCommandHandler(IProjetoService projetoService, IMapper mapper)
        {
            _projetoService = projetoService;
            _mapper = mapper;
        }

        public async Task<ProjetoDto> Handle(UpdateProjetoCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Projeto>(request.Projeto);
            var atualizado = await _projetoService.UpdateAsync(entity, request.Projeto.UsuarioId);
            return _mapper.Map<ProjetoDto>(atualizado);
        }
    }
}
