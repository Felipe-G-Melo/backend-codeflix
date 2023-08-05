using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Infra.Data.EF;
using FC.CodeFlix.Catalog.IntegrationTests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FC.CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;
public class CategoryRepositoryTestFixture
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

    public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var dbContext = new CodeflixCatalogDbContext(
                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                    .UseInMemoryDatabase("integration-tests-db")
                    .Options
        );
        if(!preserveData)
            dbContext.Database.EnsureDeleted();

        return dbContext;
    }
}

[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollection
    : ICollectionFixture<CategoryRepositoryTestFixture>
{ }
