using ProjectTest.Application.DTO;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.Interfaces
{
    public interface ITarefaService
    {
        Task<List<Tarefa>> GetAllAsync();

        Task<Tarefa> CreateAsync(Tarefa entity);

        Task<Tarefa> GetById(string id);

        Task<Tarefa> UpdateAsync(Tarefa tarefaEditada, Guid usuarioId);

        Task<bool> DeleteAsync(Guid id);
        Task<List<Tarefa>> GetTarefaByProjetoIdAsync(Guid projetoId);
        Task<bool> RemoverTarefaDoProjetoAsync(Guid tarefaId, Guid projetoId, Guid usuarioId);
        Task<List<RelatorioDesempenhoDto>> GetRelatorioDesempenhoAsync();

    }
}
