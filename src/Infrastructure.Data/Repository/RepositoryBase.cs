using Infrastructure.Data.Context;

namespace Infrastructure.Data.Repository;
public abstract class RepositoryBase<TEntity>(AppDbContext context) where TEntity : class
{
    protected readonly AppDbContext _context = context;

    public async Task<TEntity> GetByIdAsync(int id)
        => await _context.Set<TEntity>().FindAsync(id);

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
