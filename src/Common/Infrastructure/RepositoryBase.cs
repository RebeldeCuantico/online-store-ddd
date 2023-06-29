using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : AggregateRoot
    {
        protected DbSet<T> entity;

        public RepositoryBase(IDbContext context)
        {
            entity = context.Set<T>();
        }

        public void SetDates(T aggregate)
        {
            var now = DateTime.UtcNow;
            entity.Entry(aggregate).Property("CreateDate").CurrentValue = now;
            SetUpdateDate(aggregate, now);
        }

        public void SetDeleteDate(T aggregate)
        {
            var now = DateTime.UtcNow;
            SetUpdateDate(aggregate, now);
            entity.Entry(aggregate).Property("DeleteDate").CurrentValue = now;
        }

        public void SetUpdateDate(T aggregate, DateTime updateDate)
        {
            entity.Entry(aggregate).Property("UpdateDate").CurrentValue = updateDate;
        }
    }
}
