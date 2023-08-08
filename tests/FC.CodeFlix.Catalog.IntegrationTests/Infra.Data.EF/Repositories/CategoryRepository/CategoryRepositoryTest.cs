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

    [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersintenceEmpty))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    public async Task SearchReturnsEmptyWhenPersintenceEmpty()
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var repository = new InfraRepositories.CategoryRepository(context);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Desc);

        var output = await repository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(SearchReturnsPaginated))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnsPaginated(
        int quantityCategoriesToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems
    )
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var listCategories = _fixture.GetListCategories(quantityCategoriesToGenerate);
        await context.AddRangeAsync(listCategories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new InfraRepositories.CategoryRepository(context);
        var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Desc);

        var output = await repository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(quantityCategoriesToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);
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

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(
    string search,
    int page,
    int perPage,
    int expectedQuantityItemsReturned,
    int expetedQuantityTotalItems
    )
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var listCategories = _fixture.GetListCategoriesWithName(new List<string>
        {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future",
        });
        await context.AddRangeAsync(listCategories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new InfraRepositories.CategoryRepository(context);
        var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Desc);

        var output = await repository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expetedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);
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

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Infra.Data", "CategoryRepository")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")] 
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    public async Task SearchOrdered(
        string orderBy,
        string order
        )
    {
        CodeflixCatalogDbContext context = _fixture.CreateDbContext();
        var listCategories = _fixture.GetListCategories();
        await context.AddRangeAsync(listCategories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new InfraRepositories.CategoryRepository(context);
        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

        var output = await repository.Search(searchInput, CancellationToken.None);

        var expectedOrderedList = _fixture.CloneOrdered(listCategories, orderBy, searchOrder);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(listCategories.Count);
        output.Items.Should().HaveCount(listCategories.Count);
        for (int i = 0; i < expectedOrderedList.Count; i++)
        {
            var expectedItem = expectedOrderedList[i];
            var item = output.Items[i];
            item.Should().NotBeNull();
            item.Name.Should().Be(expectedItem!.Name);
            item.Id.Should().Be(expectedItem.Id);
            item.Description.Should().Be(expectedItem.Description);
            item.IsActive.Should().Be(expectedItem.IsActive);
            item.CreatedAt.Should().Be(expectedItem.CreatedAt);
        }
    }
}
