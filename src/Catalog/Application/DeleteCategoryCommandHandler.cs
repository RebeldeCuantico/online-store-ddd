using Catalog.Domain;
using Common.Domain;
using System.Runtime.CompilerServices;

namespace Catalog.Application
{
    public class DeleteCategoryCommandHandler
    {
        private readonly ICatalogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(ICatalogRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteCategoryCommand command)
        {
            var category = await _repository.GetById(new EntityId(command.id));
            if (category is null)
            {
                return Guid.Empty;
            }

            category.Remove();

            var result = _repository.Remove(category);
            await _unitOfWork.CommitAsync();
            return result.Value;
        }
    }
}
