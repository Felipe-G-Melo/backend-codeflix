using UseCases = FC.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using Moq;
using Xunit;
using FluentAssertions;
using FC.CodeFlix.Catalog.Application.Exceptions;

namespace FC.CodeFlix.Catalog.UnitTests.Application.Category.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - UseCases")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exempleCategory = _fixture.GetValidCategory();
        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exempleCategory);
        var input = new UseCases.GetCategoryInput(exempleCategory.Id);
        var useCase = new UseCases.GetCategory(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);
        output.Should().NotBeNull();
        output.Name.Should().Be(exempleCategory.Name);
        output.Description.Should().Be(exempleCategory.Description);
        output.IsActive.Should().Be(exempleCategory.IsActive);
        output.Id.Should().Be(exempleCategory.Id);
        output.CreatedAt.Should().Be(exempleCategory.CreatedAt);
    }
    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
    [Trait("Application", "GetCategory - UseCases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exempleGuid = Guid.NewGuid();
        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(
            new NotFoundException($"Category '{exempleGuid}' not found")
        );
        var input = new UseCases.GetCategoryInput(exempleGuid);
        var useCase = new UseCases.GetCategory(repositoryMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
}
