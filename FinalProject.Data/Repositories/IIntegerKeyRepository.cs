namespace FinalProject.Data.Repositories;

public interface IIntegerKeyRepository<T>
{
    public Task<T> GetByIdAsync(int id);
}
