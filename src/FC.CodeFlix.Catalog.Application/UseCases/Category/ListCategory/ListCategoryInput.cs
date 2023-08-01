using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.ListCategory;
public class ListCategoryInput : PaginatedListInput, IRequest<ListCategoryOutput>
{
    public ListCategoryInput(
        int page = 1,
        int perPage = 10,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc
    )
        : base(page, perPage, search, sort, dir)
    {
    }
}
