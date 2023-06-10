using Catalog.Application.DTOs;
using Catalog.Domain;
using Common.Domain;

namespace Catalog.Application
{
    public class UpdateCategoryCommandHandler
    {
        private readonly ICatalogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(ICatalogRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand categoryCommand)
        {
            var category = await _repository.GetById(new EntityId(categoryCommand.id));
            if(category is null)
            {
                return null;
            }

            category.ChangeName(categoryCommand.name);
            category.ChangeDescription(categoryCommand.description);
            
            var result = _repository.Update(category);            
            await _unitOfWork.CommitAsync();


            return new CategoryDto
            {
                Id = result.Id.Value,
                Name = result.Name.Value,
                Description = result.Description.Value
            };
        }
    }
}
