using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(MediumDbContext context) : base(context)
    {
    }

    public async Task<Comment> GetByIdAsync(int id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public void Remove(Comment comment)
    {
        _context.Comments.Remove(comment);
    }

    public IEnumerable<Comment> GetCommentsForArticle(int id)
    {
        return _context.Comments
                    .Where(c => c.ArticleId == id).ToList();
    }
}
