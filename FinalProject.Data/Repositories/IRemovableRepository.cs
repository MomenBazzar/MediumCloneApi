namespace FinalProject.Data.Repositories;

public interface IRemovableRepository<T>
{
    public void Remove(T entity);
}