
using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public string RoleStandard { get; } = "Standard";
    public string RoleAdmin { get; } = "Admin";
    public UserRepository(MediumDbContext context) : base(context)
    {
    }

    public IEnumerable<Article> GetFavoriteArticles(string username)
    {
        return _context.Favorites.Where(f => f.UserUsername == username).Select(f => f.Article).ToList();
    }

    public Favorite GetFavorite(string username, int articleId)
    {
        return _context.Favorites
            .Where(f => f.UserUsername == username)
            .Where(f => f.ArticleId == articleId)
            .FirstOrDefault();
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

    public void UnfollowUser(string followerUsername, string followedUsername)
    {
        var follow = GetFollow(followerUsername, followedUsername);

        _context.Follows.Remove(follow);
    }

    public async Task FollowUserAsync(string followerUsername, string followedUsername)
    {
        await _context.Follows.AddAsync(
            new Follow
            {
                FollowerUsername = followerUsername,
                FollowedUsername = followedUsername
            });
    }

    public IEnumerable<User> GetFollowedUsersAsync(string username)
    {
        return _context.Follows.Where(f => f.FollowerUsername == username).Select(f => f.Followed).ToList();
    }

    public IEnumerable<User> GetFollowersUsersAsync(string username)
    {
        return _context.Follows.Where(f => f.FollowedUsername == username).Select(f => f.Follower).ToList();
    }
    public Follow GetFollow(string followerUsername, string followedUsername)
    {
        return _context.Follows
            .Where(f => f.FollowedUsername == followedUsername)
            .Where(f => f.FollowerUsername == followerUsername)
            .FirstOrDefault();
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
