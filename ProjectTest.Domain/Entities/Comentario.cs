using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class Comentario : EntityGuid
    {
        public string Texto { get; set; }
        public virtual Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public Guid TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }
        public override async Task<(bool isValid, List<string> messages)> Validate()
        {
            var isNameEmpty = string.IsNullOrEmpty(Texto);
            if (isNameEmpty)
            {
                MessagesToReturn.Add("Nome não pode ser vazio.");
                return new(false, MessagesToReturn);
            }


            return new(true, new());
        }
    }
}
