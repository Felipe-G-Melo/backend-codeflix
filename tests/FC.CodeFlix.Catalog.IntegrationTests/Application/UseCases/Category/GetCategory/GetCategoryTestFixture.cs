﻿using FC.CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace FC.CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory;
public class GetCategoryTestFixture 
    : CategoryUseCasesBaseFixture
{
}

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection 
    : ICollectionFixture<GetCategoryTestFixture>
{ }