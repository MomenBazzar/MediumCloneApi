namespace FinalProject.Data.Models;
public class CommentDto
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int ArticleId { get; set; }
    public string AuthorUsername { get; set; }
}
