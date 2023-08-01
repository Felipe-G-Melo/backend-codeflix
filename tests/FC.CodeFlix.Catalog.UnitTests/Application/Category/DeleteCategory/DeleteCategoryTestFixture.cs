using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.CodeFlix.Catalog.UnitTests.Application.Category.DeleteCategory;
public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public DomainEntity.Category GetValidCategory
        () => new(GetValidCategoryName(), GetValidCategoryDescription());
}


[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection
    : ICollectionFixture<DeleteCategoryTestFixture>
{
}

