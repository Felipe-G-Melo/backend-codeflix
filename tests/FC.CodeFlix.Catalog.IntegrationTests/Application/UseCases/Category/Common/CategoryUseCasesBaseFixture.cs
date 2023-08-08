using DomainEntity =  FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.IntegrationTests.Common;

namespace FC.CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
public class CategoryUseCasesBaseFixture 
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

    public DomainEntity.Category GetCategory
    () => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetValidCategoryIsActive());

    public List<DomainEntity.Category> GetListCategories(int length = 10)
        => Enumerable.Range(1, length).Select(_ => GetCategory()).ToList();
}
