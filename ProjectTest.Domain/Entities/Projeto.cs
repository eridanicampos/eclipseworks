using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class Projeto : EntityGuid
    {
        public string Nome { get; set; }
        public virtual Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Tarefa>? Tarefas { get; set; }

        public override async Task<(bool isValid, List<string> messages)> Validate()
        {
            var isNameEmpty = string.IsNullOrEmpty(Nome);
            if (isNameEmpty)
            {
                MessagesToReturn.Add("Nome não pode ser vazio.");
                return new(false, MessagesToReturn);
            }


            return new(true, new());
        }
    }
}
