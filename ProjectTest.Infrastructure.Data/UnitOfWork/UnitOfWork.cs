using Microsoft.Extensions.DependencyInjection;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Domain.Interfaces.Repository;
using ProjectTest.Infrastructure.Data.Context;
using ProjectTest.Infrastructure.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace ProjectTest.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ProjectTestContext _context;
        private readonly IServiceProvider _services;
        private bool _disposed = false;

        public UnitOfWork(ProjectTestContext context, IServiceProvider services)
        {
            _context = context;
            _services = services;
        }

        public IUserRepository UserRepository => _services.GetRequiredService<IUserRepository>();
        public IAcessoUsuarioRepository AcessoUsuarioRepository => _services.GetRequiredService<IAcessoUsuarioRepository>();
        public ITarefaRepository TarefaRepository => _services.GetRequiredService<ITarefaRepository>();
        public IProjetoRepository ProjetoRepository => _services.GetRequiredService<IProjetoRepository>();
        public IHistoricoAlteracaoRepository HistoricoAlteracaoRepository => _services.GetRequiredService<IHistoricoAlteracaoRepository>();
        public IComentarioRepository ComentarioRepository => _services.GetRequiredService<IComentarioRepository>();

        public async Task CommitAsync()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            await _context.SaveChangesAsync();
        }

        public void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
