using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.CodeFlix.Catalog.UnitTests.Application.Category.ListCategory;
public class ListCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public DomainEntity.Category GetCategory
        () => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetValidCategoryIsActive());

    public List<DomainEntity.Category> GetExampleCategoriesList(int lenght = 10)
    {
        var list = new List<DomainEntity.Category>();
        for (int i = 0; i < lenght; i++)
        {
            var category = GetCategory();
            list.Add(category);
        }

        return list;
    }
}

[CollectionDefinition(nameof(ListCategoryTestFixture))]
public class ListCategoryTestFixtureCollection
    : ICollectionFixture<ListCategoryTestFixture>
{
}
