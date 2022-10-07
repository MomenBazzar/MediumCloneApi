
using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public string RoleStandard { get; } = "Standard";
    public string RoleAdmin { get; } = "Admin";
    public UserRepository(MediumDbContext context) : base(context)
    {
    }

    public async Task AddArticleToFavotiesAsync(string username, int articleId)
    {
        await _context.Favorites.AddAsync(new Favorite
        {
            UserUsername = username,
            ArticleId = articleId
        });
    }

    public void RemoveArticleFromFavoties(string username, int articleId)
    {

        _context.Favorites.Remove(
            new Favorite
            {
                ArticleId = articleId,
                UserUsername = username
            });
    }

    public void UnfollowUser(string followerUsername, string followingUsername)
    {
        _context.Follows.Remove(
            new Follow
            {
                FollowedUsername = followingUsername,
                FollowerUsername = followerUsername
            });
    }

    public async Task FollowUserAsync(string followerUsername, string followingUsername)
    {
        await _context.Follows.AddAsync(
            new Follow
            {
                FollowerUsername = followerUsername,
                FollowedUsername = followingUsername
            });
    }

    public IEnumerable<User> GetFollowedUsersAsync(string username)
    {
        return _context.Follows.Where(f => f.FollowerUsername == username).Select(f => f.Followed).ToList();
    }

    public IEnumerable<User> GetFollowingUsersAsync(string username)
    {
        return _context.Follows.Where(f => f.FollowedUsername == username).Select(f => f.Follower).ToList();
    }

    public IEnumerable<Article> GetFavoriteArticlesAsync(string username)
    {
        return _context.Favorites.Where(f => f.UserUsername == username).Select(f => f.Article).ToList();
    }

    public void Update(User user)
    {
        _context.Update(user);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return (User)await _context.Users.FindAsync(username);
    }
}
