using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models;
public class CommentCreateDto
{
    [Required]
    public string Body { get; set; }
}
