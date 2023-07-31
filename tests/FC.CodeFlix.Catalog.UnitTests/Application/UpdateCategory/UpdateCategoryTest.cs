using FC.CodeFlix.Catalog.Application.UseCases.Category.Commun;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Moq;
using Xunit;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - UseCases")]
    public async void UpdateCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exemplaCategory = _fixture.GetCategory();
        repositoryMock.Setup(x => x.Get(
            exemplaCategory.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(exemplaCategory);
        var input = new UseCase.UpdateCategoryInput(
            exemplaCategory.Id,
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription(),
            !exemplaCategory.IsActive);
        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        repositoryMock.Verify(x => x.Get(
            exemplaCategory.Id, 
            It.IsAny<CancellationToken>()
            ), Times.Once);
        repositoryMock.Verify(x => x.Update(
            exemplaCategory, 
            It.IsAny<CancellationToken>()
            ), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
}