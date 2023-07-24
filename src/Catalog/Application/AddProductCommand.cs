namespace Catalog.Application
{
    public record AddProductCommand(string name, string description, decimal price, int stock, string productCode, Guid categoryId, string currencyCode);
}
