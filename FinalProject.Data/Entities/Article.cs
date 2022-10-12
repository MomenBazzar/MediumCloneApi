using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Entities;
public class Article
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }

    public string AuthorUsername { get; set; }
    public User Author { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Favorite> UsersLoveMe { get; set; }
}


