using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.Interfaces
{
    public interface IProjetoService
    {
        Task<Projeto> CreateAsync(Projeto projeto);
        Task<List<Projeto>> GetAllAsync();
        Task<Projeto> GetByIdAsync(string id);
        Task<Projeto> UpdateAsync(Projeto projetoEditado, Guid usuarioId);
        Task<bool> DeleteAsync(Guid projetoId);
    }
}
