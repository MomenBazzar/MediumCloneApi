namespace FinalProject.Data.Repositories;

public interface IStringKeyRepository<T>
{
    public Task<T> GetByUsernameAsync(string username);
}