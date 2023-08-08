using Bogus;
using FC.CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.CodeFlix.Catalog.IntegrationTests.Common;
public abstract class BaseFixture
{
    protected Faker Faker { get; set; }
    protected BaseFixture
        () => Faker = new Faker("pt_BR");

    public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var dbContext = new CodeflixCatalogDbContext(
                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                    .UseInMemoryDatabase("integration-tests-db")
                    .Options
        );
        if (!preserveData)
            dbContext.Database.EnsureDeleted();

        return dbContext;
    }
}
