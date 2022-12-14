using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Data.Entities;
public class Follow
{
    [Key]
    public int Id { get; set; }

    public string FollowerUsername { get; set; }

    [ForeignKey("FollowerUsername")]
    public User Follower { get; set; }

    public string FollowedUsername { get; set; }

    [ForeignKey("FollowedUsername")]
    public User Followed { get; set; }
}

