using FC.CodeFlix.Catalog.Application.Interfaces;
using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repository;
using FC.CodeFlix.Catalog.UnitTests.Commun;
using Moq;
using Xunit;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UpdateCategory;
public class UpdateCategoryTestFixture : BaseFixture
{
    public UpdateCategoryTestFixture() : base()
    { }

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
        () => new (
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetValidCategoryIsActive());

    public Mock<ICategoryRepository> GetRepositoryMock
    () => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock
        () => new();
}

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture>
{
}
