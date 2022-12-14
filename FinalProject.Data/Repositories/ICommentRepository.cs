using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public interface ICommentRepository : IGenericRepository<Comment>, IRemovableRepository<Comment>,
    IIntegerKeyRepository<Comment>
{
    public IEnumerable<Comment> GetCommentsForArticle(int id);
}
