using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Data.Entities;
public class User
{
    [Key]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Role { get; set; }

    public List<Favorite> FavoriteArticles { get; set; }
    public List<Article> Articles { get; set; }

    [InverseProperty("Follower")]
    public List<Follow> Followers { get; set; }

    [InverseProperty("Following")]
    public List<Follow> Following { get; set; }


}

