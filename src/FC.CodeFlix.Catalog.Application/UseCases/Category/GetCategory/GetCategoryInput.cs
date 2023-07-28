using FC.CodeFlix.Catalog.Application.UseCases.Category.Commun;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
public class GetCategoryInput : IRequest<CategoryModelOutput>
{
    public Guid Id { get; set; }

    public GetCategoryInput(Guid id)
    {
        Id = id;
    }
}
