using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Entities;
public class Favorite
{
    [Key]
    public int Id { get; set; }

    public string UserUsername { get; set; }
    public User User { get; set; }

    public int ArticleId { get; set; }
    public Article Article { get; set; }
}

