using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class HistoricoAlteracao : EntityGuid
    {
        public virtual Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public Guid TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }
        public ETipoAllteracao Tipo { get; set; }

        public string? JsonTarefaAntesAlterada { get; set; }  // Estado anterior da tarefa
        public string? JsonTarefaDepoisAlterada { get; set; } // Estado após a alteração
        public string? CamposAlterados { get; set; }
        public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;

        public override async Task<(bool isValid, List<string> messages)> Validate()
        {

            return new(true, new());
        }
    }
}
