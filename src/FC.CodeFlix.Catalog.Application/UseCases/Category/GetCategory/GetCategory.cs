﻿using FC.CodeFlix.Catalog.Application.UseCases.Category.Commun;
using FC.CodeFlix.Catalog.Domain.Repository;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
public class GetCategory : IGetCategory
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategory(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryModelOutput> Handle(
        GetCategoryInput request,
        CancellationToken cancellationToken
        )
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        
        return CategoryModelOutput.FromCategory(category);
    }
}
