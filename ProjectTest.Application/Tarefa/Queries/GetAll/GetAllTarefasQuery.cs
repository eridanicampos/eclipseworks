using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO;
using System.Collections.Generic;

namespace ProjectTest.Application.Tarefas.Queries
{
    public class GetAllTarefasQuery : IQuery<List<TarefaDto>>
    {
    }
}
