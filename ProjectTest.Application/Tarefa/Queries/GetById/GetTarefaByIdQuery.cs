using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using System;

namespace ProjectTest.Application.Tarefas.Queries
{
    public class GetTarefaByIdQuery : IQuery<TarefaDto>
    {
        public string TarefaId { get; set; }

        public GetTarefaByIdQuery(string tarefaId)
        {
            TarefaId = tarefaId;
        }
    }
}
