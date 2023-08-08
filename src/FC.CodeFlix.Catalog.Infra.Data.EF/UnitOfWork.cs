using FC.CodeFlix.Catalog.Application.Interfaces;

namespace FC.CodeFlix.Catalog.Infra.Data.EF;
public class UnitOfWork : IUnitOfWork
{
    private readonly CodeflixCatalogDbContext _dbContext;

    public UnitOfWork(CodeflixCatalogDbContext dbContext)
    {
        _dbContext = dbContext; 
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
