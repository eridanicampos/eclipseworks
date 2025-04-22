using ProjectTest.Application.Abstractions.Messaging;
using AutoMapper;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.Projetos.Commands
{
    public class CreateProjetoCommandHandler : ICommandHandler<CreateProjetoCommand, ProjetoDto>
    {
        private readonly IProjetoService _projetoService;
        private readonly IMapper _mapper;

        public CreateProjetoCommandHandler(IProjetoService projetoService, IMapper mapper)
        {
            _projetoService = projetoService;
            _mapper = mapper;
        }

        public async Task<ProjetoDto> Handle(CreateProjetoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var projetoEntity = _mapper.Map<Projeto>(request.Projeto);

                var id = await _projetoService.CreateAsync(projetoEntity);

                var result = _mapper.Map<ProjetoDto>(projetoEntity);

                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
