using AutoFixture;
using Catalog.Domain;
using Catalog.Infrastructure.Context;
using Common.Domain;

namespace Catalog.Application
{
    public class AddCategoryCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICatalogRepository _repository;

        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, ICatalogRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Guid> Handle(AddCategoryCommand command)
        {
            var name = new Name(command.name);
            var description = new Description(command.description);
            var id = new EntityId(Guid.NewGuid());
            var category = new Category(name, description, id);

            var entityId = _repository.AddCategory(category);

            await _unitOfWork.CommitAsync();
            return entityId.Value;
        }
    }
}
