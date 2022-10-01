using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(MediumDbContext context) : base(context)
    {
    }

    public void Remove(Comment comment)
    {
        _context.Comments.Remove(comment);
    }

}
