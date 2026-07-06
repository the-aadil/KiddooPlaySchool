using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Interfaces;
using KiddooPlaySchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KiddooPlaySchool.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _dbSet.AsQueryable();
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(T entity)
    {
        entity.Id = Guid.CreateVersion7();
        entity.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;

        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;

        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        return Task.CompletedTask;
    }
}
