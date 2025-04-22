using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO
{
    public class CreateComentarioDto
    {
        public string Texto { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
