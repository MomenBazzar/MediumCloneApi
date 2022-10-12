using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models;
public class UserLoginDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}