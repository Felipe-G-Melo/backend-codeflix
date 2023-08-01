using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.CodeFlix.Catalog.UnitTests.Application.Category.GetCategory;
public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public DomainEntity.Category GetValidCategory
        () => new(GetValidCategoryName(), GetValidCategoryDescription());
}

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture>
{
}