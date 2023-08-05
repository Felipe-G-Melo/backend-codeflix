using FC.CodeFlix.Catalog.Application.Exceptions;
using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repository;
using FC.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace FC.CodeFlix.Catalog.Infra.Data.EF.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly CodeflixCatalogDbContext _context;
    private DbSet<Category> _categories => _context.Set<Category>();

    public CategoryRepository(CodeflixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task Insert(
        Category aggregate,
        CancellationToken cancellationToken
    )
    {
       await _categories.AddAsync(aggregate, cancellationToken);
    }

    public Task Update(Category aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_categories.Update(aggregate));
    }

    public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var total = await _categories.CountAsync();
        var items = await _categories.ToListAsync();
        return new (input.Page, input.PerPage, total, items);
    }

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var category =  await _categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        NotFoundException.ThrowIfNull(category, $"Category '{id}' not found");

        return category!;
    }

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_categories.Remove(aggregate));
    }
}
