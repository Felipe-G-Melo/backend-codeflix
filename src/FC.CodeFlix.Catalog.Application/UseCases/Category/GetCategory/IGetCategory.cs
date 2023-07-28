using FC.CodeFlix.Catalog.Application.UseCases.Category.Commun;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
{
}
