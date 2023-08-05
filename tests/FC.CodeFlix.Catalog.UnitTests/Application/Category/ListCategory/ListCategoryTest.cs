using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Category.ListCategory;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using Moq;
using Xunit;
using FluentAssertions;
using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace FC.CodeFlix.Catalog.UnitTests.Application.Category.ListCategory;

[Collection(nameof(ListCategoryTestFixture))]
public class ListCategoryTest
{
    private readonly ListCategoryTestFixture _fixture;

    public ListCategoryTest(ListCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(List))]
    [Trait("Application", "ListCategory - UseCases")]
    public async Task List()
    {
        var categoryList = _fixture.GetExampleCategoriesList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = new UseCase.ListCategoryInput(
                page: 2,
                perPage: 15,
                search: "search-example",
                sort: "name",
                dir: SearchOrder.Asc
            );
        var outPutRepositorySearch = new SearchOutput<DomainEntity.Category>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (IReadOnlyList<DomainEntity.Category>)categoryList,
                total: 70
          );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
          )).ReturnsAsync(outPutRepositorySearch);

        var useCase = new UseCase.ListCategory(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outPutRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outPutRepositorySearch.PerPage);
        output.Total.Should().Be(outPutRepositorySearch.Total);
        output.Items.Should().HaveCount(outPutRepositorySearch.Items.Count);
        ((List<CategoryModelOutput>)output.Items).ForEach(item =>
        {
            var exempleCategory = categoryList.FirstOrDefault(x => x.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(exempleCategory!.Name);
            item.Description.Should().Be(exempleCategory.Description);
            item.IsActive.Should().Be(exempleCategory.IsActive);
            item.CreatedAt.Should().Be(exempleCategory.CreatedAt);
        });
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
          ), Times.Once);
    }

    [Fact(DisplayName = nameof(ListOkWhenEmpty))]
    [Trait("Application", "ListCategory - UseCases")]
    public async Task ListOkWhenEmpty()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = new UseCase.ListCategoryInput(
                page: 1,
                perPage: 15,
                search: "search-example",
                sort: "name",
                dir: SearchOrder.Asc
            );
        var outPutRepositorySearch = new SearchOutput<DomainEntity.Category>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: new List<DomainEntity.Category>().AsReadOnly(),
                total: 0
          );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
          )).ReturnsAsync(outPutRepositorySearch);
        var useCase = new UseCase.ListCategory(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outPutRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outPutRepositorySearch.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
          ), Times.Once);
    }

    [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
    [Trait("Application", "ListCategory - UseCases")]
    [MemberData(nameof(GetInputsWithoutAllParameters))]
    public async Task ListInputWithoutAllParameters(UseCase.ListCategoryInput input)
    {
        var categoryList = _fixture.GetExampleCategoriesList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var outPutRepositorySearch = new SearchOutput<DomainEntity.Category>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (IReadOnlyList<DomainEntity.Category>)categoryList,
                total: 70
          );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
          )).ReturnsAsync(outPutRepositorySearch);

        var useCase = new UseCase.ListCategory(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outPutRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outPutRepositorySearch.PerPage);
        output.Total.Should().Be(outPutRepositorySearch.Total);
        output.Items.Should().HaveCount(outPutRepositorySearch.Items.Count);
        ((List<CategoryModelOutput>)output.Items).ForEach(item =>
        {
            var exempleCategory = categoryList.FirstOrDefault(x => x.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(exempleCategory!.Name);
            item.Description.Should().Be(exempleCategory.Description);
            item.IsActive.Should().Be(exempleCategory.IsActive);
            item.CreatedAt.Should().Be(exempleCategory.CreatedAt);
        });
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
          ), Times.Once);
    }

    public static IEnumerable<object[]> GetInputsWithoutAllParameters()
    {
        yield return new object[] { new UseCase.ListCategoryInput() };
        yield return new object[] { new UseCase.ListCategoryInput(2) };
        yield return new object[] { new UseCase.ListCategoryInput(2, 15) };
        yield return new object[] { new UseCase.ListCategoryInput(2, 15, "search-example") };
        yield return new object[] { new UseCase.ListCategoryInput(2, 15, "search-example", "name") };
        yield return new object[] { new UseCase.ListCategoryInput(2, 15, "search-example", "name", SearchOrder.Asc) };
    }
}
