using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models;
public class UserCreateDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}