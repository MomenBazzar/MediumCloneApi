using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Data.Entities;
public class User : IdentityUser
{
    [Required]
    [PersonalData]
    public string FirstName { get; set; }

    [Required]
    [PersonalData]
    public string LastName { get; set; }

    public List<Article> Articles { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Favorite> FavoriteArticles { get; set; }
    public List<Follow> Followers { get; set; }
    public List<Follow> Followed { get; set; }


}

