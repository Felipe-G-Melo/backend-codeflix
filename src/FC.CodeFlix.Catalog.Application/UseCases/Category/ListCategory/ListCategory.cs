using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;
using FC.CodeFlix.Catalog.Domain.Repository;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.ListCategory;
public class ListCategory : IListCategory
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategory(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoryOutput> Handle(
        ListCategoryInput request,
        CancellationToken cancellationToken
    )
    {
        var searchOutput = await _categoryRepository.Search(
                new (
                    request.Page,
                    request.PerPage,
                    request.Search,
                    request.Sort,
                    request.Dir
                    ),
                cancellationToken
            );

        return new ListCategoryOutput(
                        searchOutput.CurrentPage,
                        searchOutput.PerPage,
                        searchOutput.Total,
                        searchOutput.Items.Select(CategoryModelOutput.FromCategory).ToList()
                        );
    }
}
