namespace FinalProject.Data.Repositories;
public interface IGenericRepository<T>
{
    public Task AddAsync(T entity);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task SaveAsync();
}
