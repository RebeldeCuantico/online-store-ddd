using Common.Domain;
using Wolverine;
//using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        private readonly IMessageBus _bus;

        public UnitOfWork(IDbContext dbContext, IMessageBus bus)
        {
            _dbContext = dbContext;
            _bus = bus;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            await Dispatch();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private async Task Dispatch()
        {
            var entitiesWithEvents = _dbContext.ChangeTracker.Entries<Entity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            foreach (var entityWithEvents in entitiesWithEvents)
            {
                var domainEvents = entityWithEvents.DomainEvents.ToArray();
                entityWithEvents.DomainEvents.Clear();

                foreach (var domainEvent in domainEvents)
                {
                    await _bus.PublishAsync(domainEvent);
                }
            }
        }
    }
}
