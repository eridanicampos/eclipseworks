using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;

namespace ProjectTest.Application.Projetos.Queries
{
    public class GetAllProjetosQueryHandler : IQueryHandler<GetAllProjetosQuery, List<ProjetoDto>>
    {
        private readonly IProjetoService _service;
        private readonly IMapper _mapper;

        public GetAllProjetosQueryHandler(IProjetoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<List<ProjetoDto>> Handle(GetAllProjetosQuery request, CancellationToken cancellationToken)
        {
            var projetos = await _service.GetAllAsync();
            return _mapper.Map<List<ProjetoDto>>(projetos);
        }
    }
}
