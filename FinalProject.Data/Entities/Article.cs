using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Data.Entities;
public class Article
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Body { get; set; }

    [Required]
    public string AuthorUsername { get; set; }

    [ForeignKey("AuthorUsername")]
    public User Author { get; set; }

    public List<Comment> Comments { get; set; }
}


