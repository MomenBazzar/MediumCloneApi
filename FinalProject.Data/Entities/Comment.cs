using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Data.Entities;
public class Comment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Body { get; set; }

    public string AuthorUsername { get; set; }
    public User Author { get; set; }

    public int ArticleId { get; set; }
    public Article Article { get; set; }
}
