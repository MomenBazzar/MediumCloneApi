using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;
using FinalProject.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FinalProject.Web.Controllers
{
    [Route("api/article/{articleId}/comment")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly IArticleRepository articleRepository;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public CommentsController(ICommentRepository commentRepository,
            IArticleRepository articleRepository,
            UserManager<User> userManager,
            IMapper mapper)
        {
            this.commentRepository = commentRepository;
            this.articleRepository = articleRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpGet(), AllowAnonymous]
        public ActionResult<IEnumerable<CommentDto>> GetCommentsForArticle(int articleId)
        {

            var comments = commentRepository.GetCommentsForArticle(articleId);
            var commentsDtos = mapper.Map<IEnumerable<CommentDto>>(comments);

            return Ok(commentsDtos);
        }

        [HttpGet("~/api/comment/{id}", Name = "CommentInfo"), AllowAnonymous]
        public async Task<ActionResult<CommentDto>> Get(int id)
        {
            var comment = await commentRepository.GetByIdAsync(id);
            if (comment == null) return NotFound();

            var commentDto = mapper.Map<CommentDto>(comment);
            return Ok(commentDto);
        }

        [HttpPost()]
        public async Task<IActionResult> Post(int articleId, [FromBody] CommentCreateDto commentCreate)
        {
           
            var article = await articleRepository.GetByIdAsync(articleId);
            if (article == null) return NotFound("Article Not Found");

            var user = await userManager.FindByNameAsync(User.Identity.Name);


            var comment = mapper.Map<Comment>(commentCreate);
            comment.Author = user;
            comment.Article = article;

            await commentRepository.AddAsync(comment);
            await commentRepository.SaveAsync();

            var commentDto = mapper.Map<CommentDto>(comment);
            return CreatedAtRoute("CommentInfo", new { id = commentDto.Id }, commentDto);
        }

        [HttpDelete("~/api/comment/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var username = User.Identity.Name;

            var comment = await commentRepository.GetByIdAsync(id);
            if (comment == null) return NotFound($"there are no comment found with id={id}");

            if (!string.Equals(comment.AuthorUsername, username, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("you are not allowed to Delete comments for other users");
            }

            commentRepository.Remove(comment);
            await commentRepository.SaveAsync();

            return Ok("Comment was deleted");
        }
    }
}
