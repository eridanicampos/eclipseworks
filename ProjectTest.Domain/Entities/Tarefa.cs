using ProjectTest.Domain.Entities.Enum;

namespace ProjectTest.Domain.Entities
{
    public class Tarefa : EntityGuid
    {

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public EStatusTarefa Status { get; set; } = EStatusTarefa.Pendente;
        public EPrioridade Prioridade { get; private set; }
        public Guid ProjetoId { get; set; }
        public Projeto Projeto { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<HistoricoAlteracao> Historico { get; set; }
        public ICollection<Comentario> Comentarios { get; set; }

        public Tarefa(EPrioridade prioridade)
        {
            Prioridade = prioridade;
        }
        public Tarefa()
        {
        }

        public override Task<(bool isValid, List<string> messages)> Validate()
        {
            var messages = new List<string>();

            if (string.IsNullOrEmpty(Titulo) || Titulo.Length < 5 || Titulo.Length > 100)
                messages.Add("O título deve conter entre 5 e 100 caracteres.");


            bool isValid = !messages.Any();
            return Task.FromResult((isValid, messages));
        }
    }

}
