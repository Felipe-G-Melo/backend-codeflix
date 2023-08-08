using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.IntegrationTests.Common;
using Xunit;

namespace FC.CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;
public class UnitOfWorkTestFixture 
    : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
        {
            categoryName = Faker.Commerce.Categories(1)[0];
        }
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Lorem.Paragraph();
        if (categoryDescription.Length > 10000)
            categoryDescription = categoryDescription[..10000];

        return categoryDescription;
    }

    public bool GetValidCategoryIsActive
        () => Faker.Random.Bool();

    public Category GetCategory
    () => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetValidCategoryIsActive());

    public List<Category> GetListCategories(int length = 10)
        => Enumerable.Range(1, length).Select(_ => GetCategory()).ToList();
}

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection 
    : ICollectionFixture<UnitOfWorkTestFixture>
{
}