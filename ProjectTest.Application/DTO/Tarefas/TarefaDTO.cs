using ProjectTest.Application.DTO.Comentarios;
using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO
{
    public class TarefaDto
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public DateTime DataVencimento { get; set; }

        public EStatusTarefa Status { get; set; }

        public EPrioridade Prioridade { get; set; }

        public Guid ProjetoId { get; set; }

        public string? NomeProjeto { get; set; }

        public List<ComentarioDTO>? Comentarios { get; set; }
    }
}
