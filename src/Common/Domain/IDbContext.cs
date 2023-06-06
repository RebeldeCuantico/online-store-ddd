using Microsoft.EntityFrameworkCore;

namespace Common.Domain
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        void Dispose();
    }
}
