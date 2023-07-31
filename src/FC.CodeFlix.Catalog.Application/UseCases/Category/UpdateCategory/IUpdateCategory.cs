using FC.CodeFlix.Catalog.Application.UseCases.Category.Commun;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
public interface IUpdateCategory 
    : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
{
}
