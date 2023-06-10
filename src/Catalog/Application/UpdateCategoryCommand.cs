namespace Catalog.Application
{
    public record UpdateCategoryCommand(string name, string description, Guid id);    
}
