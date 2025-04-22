namespace ProjectTest.Application.DTO.Comentarios
{
    public class UpdateComentarioDto
    {
        public Guid Id { get; set; } 
        public string Texto { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
