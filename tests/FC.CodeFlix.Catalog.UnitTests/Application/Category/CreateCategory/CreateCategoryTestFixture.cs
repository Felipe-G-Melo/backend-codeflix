using FC.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.CodeFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.CodeFlix.Catalog.UnitTests.Application.Category.CreateCategory;
public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public CreateCategoryTestFixture()
        : base()
    { }

    public CreateCategoryInput GetInput
        () => new CreateCategoryInput(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetValidCategoryIsActive());
}


[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
{
}