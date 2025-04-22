using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Repository;
using ProjectTest.Infrastructure.Data.Context;
using ProjectTest.Infrastructure.Data.Repositories.Common;

namespace ProjectTest.Infrastructure.Data.Repositories
{
    public class ComentarioRepository : GenericAsyncRepository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(ProjectTestContext _dbContext) : base(_dbContext)
        {
        }

    }
}
