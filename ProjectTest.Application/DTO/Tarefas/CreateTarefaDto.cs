using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO
{
    public class CreateTarefaDto
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public EPrioridade Prioridade { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ProjetoId { get; set; }

        public List<CreateComentarioDto>? Comentarios { get; set; }
    }
}
