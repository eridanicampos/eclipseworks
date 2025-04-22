using ProjectTest.Application.DTO.Comentarios;
using ProjectTest.Domain.Entities.Enum;

namespace ProjectTest.Application.DTO
{
    public class UpdateTarefaDto
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public EStatusTarefa Status { get; set; }

        public Guid UsuarioId { get; set; }
        public Guid ProjetoId { get; set; }

        public List<UpdateComentarioDto> Comentarios { get; set; } = new();

    }
}
