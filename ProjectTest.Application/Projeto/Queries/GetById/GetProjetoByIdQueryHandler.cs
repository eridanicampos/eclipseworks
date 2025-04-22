using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using ProjectTest.Application.Interfaces;

namespace ProjectTest.Application.Projetos.Queries
{
    public class GetProjetoByIdQueryHandler : IQueryHandler<GetProjetoByIdQuery, ProjetoDto>
    {
        private readonly IProjetoService _service;
        private readonly IMapper _mapper;

        public GetProjetoByIdQueryHandler(IProjetoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<ProjetoDto> Handle(GetProjetoByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _service.GetByIdAsync(request.Id);
            return _mapper.Map<ProjetoDto>(entity);
        }
    }
}
