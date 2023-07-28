using FC.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using UseCases = FC.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using Moq;
using Xunit;
using FC.CodeFlix.Catalog.Application.Exceptions;
using FluentAssertions;

namespace FC.CodeFlix.Catalog.UnitTests.Application.DeleteCategory;
[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory - UseCases")]
    public async Task DeleteCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var category = _fixture.GetValidCategory();
        repositoryMock.Setup(r => r.Get(
            category.Id, It.IsAny<CancellationToken>())
        ).ReturnsAsync(category);
        var input = new DeleteCategoryInput(category.Id);
        var useCase = new UseCases.DeleteCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
            );
        
        await useCase.Handle(input, CancellationToken.None);
        
        repositoryMock.Verify(x => x.Get(
                       category.Id, It.IsAny<CancellationToken>()
                       ), Times.Once);
        repositoryMock.Verify(x => x.Delete(
                   category, It.IsAny<CancellationToken>()
                   ), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "DeleteCategory - UseCases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleGuid = Guid.NewGuid();
        repositoryMock.Setup(r => r.Get(
            exampleGuid, It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found"));
        var input = new DeleteCategoryInput(exampleGuid);
        var useCase = new UseCases.DeleteCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
            );

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.Get(
                       exampleGuid, It.IsAny<CancellationToken>()
                       ), Times.Once);
    }
}
