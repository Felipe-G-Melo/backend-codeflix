using DomainEntity =  FC.CodeFlix.Catalog.Domain.Entity;
using Xunit;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;

namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryFixture;
    public CategoryTest(CategoryTestFixture categoryFixture)
    {
        _categoryFixture = categoryFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = _categoryFixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(
            validData.Name,
            validData.Description
            );

        var dateTimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > dateTimeBefore).Should().BeTrue();
        (category.CreatedAt < dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)] 
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = _categoryFixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(
            validData.Name,
            validData.Description,
            isActive
            );

        var dateTimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > dateTimeBefore).Should().BeTrue();
        (category.CreatedAt < dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validData = _categoryFixture.GetValidCategory();

        Action action = 
            () => new DomainEntity.Category(name!, validData.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name is required");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validData = _categoryFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(validData.Name, null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description is required");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThen3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("Ca")]
    [InlineData("C")]
    [InlineData("te")]
    public void InstantiateErrorWhenNameIsLessThen3Characters(string invalidName)
    {
        var validData = _categoryFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(invalidName, validData.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name must be at least 3 characters");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThen255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThen255Characters()
    {
        var validData = _categoryFixture.GetValidCategory();
        var invalidName = _categoryFixture.Faker.Lorem.Letter(256);

        Action action =
            () => new DomainEntity.Category(invalidName, validData.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name must be less or equal 255 characters");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThen10000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThen10000Characters()
    {
        var validData = _categoryFixture.GetValidCategory();
        var invalidDescription = _categoryFixture.Faker.Lorem.Letter(10001);

        Action action =
            () => new DomainEntity.Category(validData.Name, invalidDescription);

        action.Should()
           .Throw<EntityValidationException>()
           .WithMessage("Description must be less or equal 10000 characters");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validData = _categoryFixture.GetValidCategory();

        var category = new DomainEntity.Category(
            validData.Name,
            validData.Description,
            false
            );

        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validData = _categoryFixture.GetValidCategory();

        var category = new DomainEntity.Category(
            validData.Name,
            validData.Description,
            true
            );

        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = _categoryFixture.GetValidCategory();
        var newValues = new
        {
            Name = _categoryFixture.GetValidCategoryName(),
            Description = _categoryFixture.GetValidCategoryDescription()
        };

        category.Update(newValues.Name, newValues.Description);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = _categoryFixture.GetValidCategory();
        var newValues = new
        {
            Name = _categoryFixture.GetValidCategoryName(),
        };
        var currentDescription = category.Description;

        category.Update(newValues.Name);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = _categoryFixture.GetValidCategory();

        Action action =
            () => category.Update(name!);

        action.Should()
          .Throw<EntityValidationException>()
          .WithMessage("Name is required");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThen3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("Ca")]
    [InlineData("C")]
    [InlineData("te")]
    public void UpdateErrorWhenNameIsLessThen3Characters(string invalidName)
    {
        var category = _categoryFixture.GetValidCategory();

        Action action =
            () => category.Update(invalidName);

        action.Should()
          .Throw<EntityValidationException>()
          .WithMessage("Name must be at least 3 characters");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThen255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThen255Characters()
    {
        var category = _categoryFixture.GetValidCategory();

        var invalidName = _categoryFixture.Faker.Lorem.Letter(256);

        Action action =
            () => category.Update(invalidName);

        action.Should()
         .Throw<EntityValidationException>()
         .WithMessage("Name must be less or equal 255 characters");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThen10000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThen10000Characters()
    {
        var category = _categoryFixture.GetValidCategory();

        var invalidDescription = _categoryFixture.Faker.Lorem.Letter(10001);

        Action action =
            () => new DomainEntity.Category("Category new name", invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description must be less or equal 10000 characters");
    }
}