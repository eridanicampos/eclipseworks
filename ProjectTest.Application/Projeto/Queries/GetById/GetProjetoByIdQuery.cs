using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;

namespace ProjectTest.Application.Projetos.Queries
{
    public class GetProjetoByIdQuery : IQuery<ProjetoDto>
    {
        public string Id { get; }

        public GetProjetoByIdQuery(string id)
        {
            Id = id;
        }
    }
}
