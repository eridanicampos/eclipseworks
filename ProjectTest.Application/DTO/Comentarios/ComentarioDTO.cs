using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO.Comentarios
{
    public class ComentarioDTO
    {
        public Guid Id { get; set; }
        public string Texto { get; set; }

        public Guid UsuarioId { get; set; }
        public string? NomeUsuario { get; set; }

        public Guid TarefaId { get; set; }
    }
}
