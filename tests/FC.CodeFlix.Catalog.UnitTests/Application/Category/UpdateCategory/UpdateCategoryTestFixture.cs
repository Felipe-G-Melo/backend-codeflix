using FC.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.CodeFlix.Catalog.UnitTests.Application.Category.UpdateCategory;
public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public DomainEntity.Category GetCategory
        () => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetValidCategoryIsActive());

    public UpdateCategoryInput GetValidInput
        (Guid? id = null) => new(
            id ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetValidCategoryIsActive());
}

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture>
{
}
