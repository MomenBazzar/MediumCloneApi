using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public interface IUserRepository : IGenericRepository<User>, IUpdatableRepository<User>
{
    public Task FollowUserAsync(string followerUsername, string followedUsername);
    public void UnfollowUser(string followerUsername, string followedUsername);
    public IEnumerable<User> GetFollowingUsersAsync(string username);
    public IEnumerable<User> GetFollowedUsersAsync(string username);
    public Task AddArticleToFavotiesAsync(string username, int articleId);
    public void RemoveArticleFromFavoties(string username, int articleId);
    public IEnumerable<Article> GetFavoriteArticlesAsync(string username);


}
