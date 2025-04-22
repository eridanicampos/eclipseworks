using MediatR;
using ProjectTest.Application.Abstractions.Messaging;
using System;

namespace ProjectTest.Application.Projetos.Commands.DeleteProjeto
{
    public class DeleteProjetoCommand : ICommand<bool>
    {
        public Guid ProjetoId { get; }

        public DeleteProjetoCommand(Guid projetoId)
        {
            ProjetoId = projetoId;
        }
    }
}
