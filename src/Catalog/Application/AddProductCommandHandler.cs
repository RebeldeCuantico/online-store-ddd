using Catalog.Domain;
using Common.Domain;

namespace Catalog.Application
{
    public class AddProductCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _repository;

        public AddProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Guid> HandleAsync(AddProductCommand command)
        {
            var name = new Name(command.name);
            var productCode = new ProductCode(command.productCode);
            var description = new Description(command.description);
            var price = new Price(command.price);
            var stock = new AvailableStock(command.stock);
            var referenceId = new ReferenceId(command.categoryId);
            var id = new EntityId(Guid.NewGuid());

            var product = new Product(id, name, productCode, description, price, stock, referenceId);

            _repository.AddProduct(product);

            await _unitOfWork.CommitAsync();

            return product.Id.Value;
        }
    }
}
