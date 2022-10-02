using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly MediumDbContext _context;

    public GenericRepository(MediumDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}