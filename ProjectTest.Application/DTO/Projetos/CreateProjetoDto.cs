namespace ProjectTest.Application.DTO
{
    public class CreateProjetoDto
    {
        public string Nome { get; set; }
        public Guid UsuarioId { get; set; }

        public List<CreateTarefaDto>? Tarefas { get; set; }
    }
}
