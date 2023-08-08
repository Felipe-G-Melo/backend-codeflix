using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using FC.CodeFlix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using FluentAssertions;
using FC.CodeFlix.Catalog.Application.Exceptions;

namespace FC.CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Integration/Application", "GetCategory - UseCases")]
    public async Task GetCategory()
    {
        var exempleCategory = _fixture.GetCategory();
        var dbContext = _fixture.CreateDbContext();
        dbContext.Categories.Add(exempleCategory);
        dbContext.SaveChanges();
        var repository = new CategoryRepository(dbContext);
        var input = new UseCase.GetCategoryInput(exempleCategory.Id);
        var useCase = new UseCase.GetCategory(repository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(exempleCategory.Name);
        output.Description.Should().Be(exempleCategory.Description);
        output.IsActive.Should().Be(exempleCategory.IsActive);
        output.Id.Should().Be(exempleCategory.Id);
        output.CreatedAt.Should().Be(exempleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
    [Trait("Integration/Application", "GetCategory - UseCases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        var exempleCategory = _fixture.GetCategory();
        var dbContext = _fixture.CreateDbContext();
        dbContext.Categories.Add(exempleCategory);
        dbContext.SaveChanges();
        var repository = new CategoryRepository(dbContext);
        var input = new UseCase.GetCategoryInput(Guid.NewGuid());
        var useCase = new UseCase.GetCategory(repository);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{input.Id}' not found");
    }
}
