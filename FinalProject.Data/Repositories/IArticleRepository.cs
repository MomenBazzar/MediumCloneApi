using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public interface IArticleRepository : IGenericRepository<Article>,IUpdatableRepository<Article>, 
    IRemovableRepository<Article>, IIntegerKeyRepository<Article>
{
    public IEnumerable<Article> GetForUser(string username);
}
