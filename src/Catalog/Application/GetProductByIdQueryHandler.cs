using Catalog.Application.DTOs;
using Catalog.Domain;

namespace Catalog.Application
{
    public class GetProductByIdQueryHandler
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> HandleAsync(GetProductByIdQuery query)
        {

            var product = await _productRepository.GetProductById(new Common.Domain.EntityId(query.productId));
            if (product == null) return null;

            return (ProductDto)product;
        }
    }
}
