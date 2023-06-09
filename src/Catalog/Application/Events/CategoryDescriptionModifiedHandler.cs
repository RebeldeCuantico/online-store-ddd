using Catalog.Domain.DomainEvents;

namespace Catalog.Application.Events
{
    public class CategoryDescriptionModifiedHandler
    {
        public async Task Handle(CategoryDescriptionModified categoryDescriptionModified)
        {
            //TODO : Stuff
            await Task.FromResult(categoryDescriptionModified);
        }
    }
}
