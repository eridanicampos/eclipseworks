using System;

namespace ProjectTest.Application.DTO.Projetos
{
    public class UpdateProjetoDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
