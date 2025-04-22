using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO
{
    public class ProjetoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public Guid UsuarioId { get; set; }
        public string? NomeUsuario { get; set; }

        public List<TarefaDto>? Tarefas { get; set; }
    }
}
