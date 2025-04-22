using Microsoft.EntityFrameworkCore;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;
using ProjectTest.Domain.Interfaces.Repository;
using ProjectTest.Infrastructure.Data.Context;
using ProjectTest.Infrastructure.Data.Repositories.Common;

namespace ProjectTest.Infrastructure.Data.Repositories
{
    public class TarefaRepository : GenericAsyncRepository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(ProjectTestContext _dbContext) : base(_dbContext)
        {
        }

        public async Task<List<Tarefa>> GetTarefasConcluidasComUsuarioAsync(DateTime dataLimite)
        {
            return await DbSet
                .Include(t => t.Usuario)
                .Where(t => t.Status == EStatusTarefa.Concluida && t.UpdateAt >= dataLimite && !t.IsDeleted)
                .ToListAsync();
        }



        public async Task<List<Tarefa>> GetTarefaByProjetoIdAsync(Guid projetoId)
        {
            var tarefas = await DbSet
                    .Include(x => x.Projeto)
                    .Include(x => x.Comentarios)
                    .Where(x => x.ProjetoId == projetoId && !x.Projeto.IsDeleted && !x.IsDeleted)
                    .ToListAsync();

            return tarefas;
        }
    }
}
