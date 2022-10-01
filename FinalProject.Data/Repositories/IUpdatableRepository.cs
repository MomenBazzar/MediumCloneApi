namespace FinalProject.Data.Repositories;

public interface IUpdatableRepository<T>
{
    public void Update(T entity);
}