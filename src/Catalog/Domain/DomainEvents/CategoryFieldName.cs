namespace Catalog.Domain.DomainEvents
{
    public class CategoryFieldName
    {        
        public static readonly CategoryFieldName CategoryName = new(1,"Name");
        public static readonly CategoryFieldName CategoryDescription = new(2, "Description");

        public int Id { get; }

        public string Name { get; }

        public CategoryFieldName(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static readonly IReadOnlyList<CategoryFieldName> CategoryFields = new List<CategoryFieldName> 
        {
            CategoryName,
            CategoryDescription
        };
    }
}
