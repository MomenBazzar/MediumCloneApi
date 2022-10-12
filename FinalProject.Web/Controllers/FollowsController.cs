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
    [Route("api/{username}")]
    [ApiController]
    [Authorize]
    public class FollowsController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public FollowsController(UserManager<User> userManager,
            IMapper mapper,
            IUserAuthenticationManager authenticationManager,
            IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        [HttpGet("follower")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetFollowers(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("user not found");

            var followers = userRepository.GetFollowersUsersAsync(username);

            var followersDto = mapper.Map<IEnumerable<UserDto>>(followers);
            return Ok(followersDto);
        }

        [HttpGet("following")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetFollowingUsers(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("user not found");

            var followed = userRepository.GetFollowedUsersAsync(username);

            var followedDto = mapper.Map<IEnumerable<UserDto>>(followed);
            return Ok(followedDto);
        }

        [HttpPost("Follower")]
        public async Task<IActionResult> AddFollow(string username)
        {
            var followed = await userManager.FindByNameAsync(username);
            if (followed == null) return NotFound("username not found");

            var followerUsername = User.Identity.Name;

            var follow = userRepository.GetFollow(followerUsername, username);
            if (follow != null) return BadRequest("you already follow this user");

            await userRepository.FollowUserAsync(followerUsername, username);
            await userRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("Follower")]
        public async Task<IActionResult> Unfollow(string username)
        {
            var followed = await userManager.FindByNameAsync(username);
            if (followed == null) return NotFound("username not found");

            var followerUsername = User.Identity.Name;

            var follow = userRepository.GetFollow(followerUsername, username);
            if (follow == null) return BadRequest("you are not following this user");

            userRepository.UnfollowUser(followerUsername, username);
            await userRepository.SaveAsync();
            return NoContent();
        }
    }
}