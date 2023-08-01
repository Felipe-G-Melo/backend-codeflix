using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.ListCategory;
public class ListCategoryOutput : PaginatedListOutput<CategoryModelOutput>
{
    public ListCategoryOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<CategoryModelOutput> items
    )   : base(page, perPage, total, items)
    {
    }
}
