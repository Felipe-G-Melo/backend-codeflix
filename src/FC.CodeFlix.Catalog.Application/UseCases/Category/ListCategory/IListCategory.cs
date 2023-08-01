using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.ListCategory;
public interface IListCategory 
    : IRequestHandler<ListCategoryInput, ListCategoryOutput>
{
}
