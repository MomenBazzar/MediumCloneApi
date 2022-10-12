using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public interface IUserRepository : IGenericRepository<User>, IUpdatableRepository<User>, IStringKeyRepository<User>
{
    public string RoleStandard { get; }
    public string RoleAdmin { get; }

    public Task FollowUserAsync(string followerUsername, string followedUsername);
    public void UnfollowUser(string followerUsername, string followedUsername);
    public Follow GetFollow(string followerUsername, string followedUsername);
    public IEnumerable<User> GetFollowersUsersAsync(string username);
    public IEnumerable<User> GetFollowedUsersAsync(string username);
    public Task AddArticleToFavotiesAsync(string username, int articleId);
    public void RemoveArticleFromFavoties(string username, int articleId);
    public IEnumerable<Article> GetFavoriteArticles(string username);
    public Favorite GetFavorite(string username, int articleId);
}
