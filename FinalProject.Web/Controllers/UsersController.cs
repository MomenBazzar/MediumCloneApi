using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;
using FinalProject.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FinalProject.Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IUserAuthenticationManager authenticationManager;

        public UsersController(UserManager<User> userManager, 
            IMapper mapper,
            IUserAuthenticationManager authenticationManager)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.authenticationManager = authenticationManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = userManager.Users.ToList();
            var usersDto = mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(usersDto);
        }

        [HttpGet("@{username}", Name = "UserInfo"), AllowAnonymous]
        public async Task<ActionResult<UserDto>> Get(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found");

            var userDto = mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpPost("~/api/Register"), AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(UserCreateDto userCreate)
        {
            var userResult = await authenticationManager.RegisterUserAsync(userCreate);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : new CreatedResult("UserInfo", userCreate);
            
        }

        [HttpPost("~/api/Login"), AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserLoginDto userLogin)
        {
            return !await authenticationManager.ValidateUserAsync(userLogin)
            ? Unauthorized()
            : Ok(new { Token = await authenticationManager.CreateTokenAsync() });

        }


        [HttpPut("@{username}")]
        public async Task<ActionResult<string>> UpdateUser(string username, [FromBody] UserUpdateDto userUpdate)
        {
            if (!string.Equals(User.Identity.Name, username, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("You are not allowed to update this object");
            }

            var user = await userManager.FindByNameAsync(username);
            
            user.Email = userUpdate.Email;
            user.FirstName = userUpdate.FirstName;
            user.LastName = userUpdate.LastName;
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, userUpdate.Password);

            var result = await userManager.UpdateAsync(user);
            return !result.Succeeded ? new BadRequestObjectResult(result) : NoContent();
        }

    }
}
