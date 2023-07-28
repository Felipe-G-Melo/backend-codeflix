using FC.CodeFlix.Catalog.Application.Interfaces;
using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repository;
using FC.CodeFlix.Catalog.UnitTests.Commun;
using Moq;
using Xunit;

namespace FC.CodeFlix.Catalog.UnitTests.Application.DeleteCategory;
public class DeleteCategoryTestFixture : BaseFixture
{
    public DeleteCategoryTestFixture() 
        : base()
    {}

    public string GetValidName()
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

    public string GetValidDescription()
    {
        var categoryDescription = Faker.Lorem.Paragraph();
        if (categoryDescription.Length > 10000)
            categoryDescription = categoryDescription[..10000];

        return categoryDescription;
    }

    public Category GetValidCategory
        () => new(GetValidName(), GetValidDescription());

    public Mock<ICategoryRepository> GetRepositoryMock
        () => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock
        () => new();
}


[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection 
    : ICollectionFixture<DeleteCategoryTestFixture>
{
}

