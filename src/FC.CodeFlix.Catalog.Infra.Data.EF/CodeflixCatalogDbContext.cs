using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FC.CodeFlix.Catalog.Infra.Data.EF;
public class CodeflixCatalogDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();

    public CodeflixCatalogDbContext(
        DbContextOptions<CodeflixCatalogDbContext> options
        ) 
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}
