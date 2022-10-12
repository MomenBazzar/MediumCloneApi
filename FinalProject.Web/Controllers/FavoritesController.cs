using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;
using FinalProject.Data.Repositories;
using FinalProject.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FinalProject.Web.Controllers
{
    [Route("api/{username}/favorite-articles")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public FavoritesController(UserManager<User> userManager,
            IMapper mapper,
            IUserAuthenticationManager authenticationManager,
            IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetFavorites(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("user not found");

            var favorites = userRepository.GetFavoriteArticles(username);

            var favoritesDto = mapper.Map<IEnumerable<ArticleDto>>(favorites);
            return Ok(favoritesDto);
        }

        [HttpPost("{articleId}")]
        public async Task<IActionResult> AddFavorite(string username, int articleId)
        {
            if (!string.Equals(User.Identity.Name, username, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("You are not allowed to update this object");
            }

            Favorite favorite = userRepository.GetFavorite(username, articleId);
            if (favorite == null) return BadRequest("you already love this article");

            await userRepository.AddArticleToFavotiesAsync(username, articleId);
            await userRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{articleId}")]
        public async Task<IActionResult> RemoveFavorite(string username, int articleId)
        {
            if (!string.Equals(User.Identity.Name, username, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("You are not allowed to update this object");
            }

            Favorite favorite = userRepository.GetFavorite(username, articleId);
            if (favorite != null) return BadRequest("you don't like this article anyway");

            userRepository.RemoveArticleFromFavoties(username, articleId);
            await userRepository.SaveAsync();
            return NoContent();
        }
    }
}