using Common.Infrastructure;

namespace Catalog.Application.DTOs
{
    public class CategoryDto : IMessage
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
