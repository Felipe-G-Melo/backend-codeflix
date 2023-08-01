using FC.CodeFlix.Catalog.Application.Interfaces;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repository;
using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
public class CreateCategory : ICreateCategory
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategory(
          ICategoryRepository repository,
          IUnitOfWork unitOfWork
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryModelOutput> Handle(
        CreateCategoryInput input,
        CancellationToken cancellationToken
        )
    {
        var category = new DomainEntity.Category(
            input.Name,
            input.Description!,
            input.IsActive
            );

        await _repository.Insert(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
