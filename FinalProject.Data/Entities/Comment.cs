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

    [ForeignKey("AuthorUsername")]
    public User Author { get; set; }

    [Required]
    public int ArticleId { get; set; }

    [ForeignKey("ArticleId")]
    public Article Article { get; set; }
}
