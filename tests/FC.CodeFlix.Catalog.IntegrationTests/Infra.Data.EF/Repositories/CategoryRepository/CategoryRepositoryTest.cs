using FC.CodeFlix.Catalog.Infra.Data.EF;
using InfraRepositories = FC.CodeFlix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Xunit;
using FC.CodeFlix.Catalog.Application.Exceptions;
using FC.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    public async Task Insert()
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var category = _fixture.GetCategory();
        var repository = new InfraRepositories.CategoryRepository(context);

        await repository.Insert(category, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(category.Id);
        categoryDb.Should().NotBeNull();
        categoryDb!.Name.Should().Be(category.Name);
        categoryDb.Description.Should().Be(category.Description);
        categoryDb.IsActive.Should().Be(category.IsActive);
        categoryDb.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    public async Task Get()
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var category = _fixture.GetCategory();
        var listCategories = _fixture.GetListCategories();
        listCategories.Add(category);
        await context.AddRangeAsync(listCategories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new InfraRepositories.CategoryRepository(_fixture.CreateDbContext(true));

        var categoryDb = await repository.Get(category.Id, CancellationToken.None);

        categoryDb.Should().NotBeNull();
        categoryDb!.Name.Should().Be(category.Name);
        categoryDb.Id.Should().Be(category.Id);
        categoryDb.Description.Should().Be(category.Description);
        categoryDb.IsActive.Should().Be(category.IsActive);
        categoryDb.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    public async Task GetThrowIfNotFound()
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var listCategories = _fixture.GetListCategories();
        await context.AddRangeAsync(listCategories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new InfraRepositories.CategoryRepository(context);
        var exampleId = Guid.NewGuid();

        var task = async () => await repository.Get(exampleId, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{exampleId}' not found");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    public async Task Update()
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var category = _fixture.GetCategory();
        var newCategoryValues = _fixture.GetCategory();
        var listCategories = _fixture.GetListCategories();
        listCategories.Add(category);
        await context.AddRangeAsync(listCategories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new InfraRepositories.CategoryRepository(context);

        category.Update(newCategoryValues.Name, newCategoryValues.Description);
        await repository.Update(category, CancellationToken.None);
        await context.SaveChangesAsync();

        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(category.Id);
        categoryDb.Should().NotBeNull();
        category.Name.Should().Be(category.Name);
        category.Id.Should().Be(category.Id);
        category.Description.Should().Be(category.Description);
        category.IsActive.Should().Be(category.IsActive);
        category.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    public async Task Delete()
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var category = _fixture.GetCategory();
        var listCategories = _fixture.GetListCategories();
        listCategories.Add(category);
        await context.AddRangeAsync(listCategories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new InfraRepositories.CategoryRepository(context);

        await repository.Delete(category, CancellationToken.None);
        await context.SaveChangesAsync();

        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(category.Id);
        categoryDb.Should().BeNull();
    }

    [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    public async Task SearchReturnsListAndTotal()
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var listCategories = _fixture.GetListCategories();
        await context.AddRangeAsync(listCategories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new InfraRepositories.CategoryRepository(context);
        var searchInput = new SearchInput(1,20,"","",SearchOrder.Desc);

        var output = await repository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(listCategories.Count);
        output.Items.Should().HaveCount(listCategories.Count);
        foreach (Category item in output.Items)
        {
            var exampleItem = listCategories.Find(category => category.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(exampleItem!.Name);
            item.Description.Should().Be(exampleItem.Description);
            item.IsActive.Should().Be(exampleItem.IsActive);
            item.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }
}
