using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;
using FinalProject.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FinalProject.Web.Controllers
{
    [Route("api/article")]
    [ApiController]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository articleRepository;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public ArticlesController(IArticleRepository articleRepository,
            UserManager<User> userManager,
            IMapper mapper)
        {
            this.articleRepository = articleRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetAll()
        {
            var articles = await articleRepository.GetAllAsync();
            var articleDtos = mapper.Map<IEnumerable<ArticleDto>>(articles);
            return Ok(articleDtos);
        }

        [HttpGet("~/api/user/{username}/article"), AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetAllForUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("Username not found");

            var articles = articleRepository.GetForUser(username);
            var articleDtos = mapper.Map<IEnumerable<ArticleDto>>(articles);

            return Ok(articleDtos);
        }

        [HttpGet("{id}", Name = "ArticleInfo"), AllowAnonymous]
        public async Task<ActionResult<ArticleDto>> Get(int id)
        {
            var article = await articleRepository.GetByIdAsync(id);
            if (article == null) return NotFound();

            var articleDto = mapper.Map<ArticleDto>(article);   
            return Ok(articleDto);
        }

        [HttpPost("~/api/user/{username}/article")]
        public async Task<IActionResult> Post(string username, [FromBody] ArticleCreateDto articleCreate)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User");

            if (!string.Equals(User.Identity.Name, username, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("you are not allowed to add articles for other users");
            }
            
            var article = mapper.Map<Article>(articleCreate);
            article.Author = user;

            await articleRepository.AddAsync(article);
            await articleRepository.SaveAsync();

            var articleDto = mapper.Map<ArticleDto>(article);
            return CreatedAtRoute("ArticleInfo", new { id = articleDto.Id }, articleDto);
        }

        [HttpPut("~/api/user/{username}/article/{id}")]
        public async Task<IActionResult> Put(string username, int id, [FromBody] ArticleUpdateDto articleUpdate)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User");

            if (!string.Equals(User.Identity.Name, username, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("you are not allowed to update articles for other users");
            }

            var article = await articleRepository.GetByIdAsync(id);
            if (article == null) return NotFound($"there are no article found with id={id}");

            article.Title = articleUpdate.Title;
            article.Body = articleUpdate.Body;

            articleRepository.Update(article);
            await articleRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("~/api/user/{username}/article/{id}")]
        public async Task<IActionResult> Delete(string username, int id)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User");

            if (!string.Equals(User.Identity.Name, username, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("you are not allowed to Delete articles for other users");
            }

            var article = await articleRepository.GetByIdAsync(id);
            if (article == null) return NotFound($"there are no article found with id={id}");
            articleRepository.Remove(article);
            await articleRepository.SaveAsync();

            return Ok("Article was deleted");
        }
    }
}
