using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models;
public class ArticleCreateDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Body { get; set; }
}
