using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;
using FinalProject.Data.Repositories;
using FinalProject.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;

        public UsersController(
            IUserRepository repository, IMapper mapper,
            IConfiguration configuration,
            IJwtAuthenticationManager authenticationManager)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.jwtAuthenticationManager = authenticationManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await repository.GetAllAsync();

            var usersDto = mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(usersDto);
        }

        [HttpGet("@{username}", Name = "UserInfo"), Authorize]
        public async Task<ActionResult<UserDto>> Get(string username)
        {
            var userFromRepo = await repository.GetByUsernameAsync(username);
            if (userFromRepo == null) return NotFound();

            var userDto = mapper.Map<UserDto>(userFromRepo);

            return Ok(userDto);
        }

        [HttpPost("~/api/Register"), AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(UserCreateDto userCreate)
        {
            var userFromRepo = await repository.GetByUsernameAsync(userCreate.Username);
            if (userFromRepo != null) return Conflict($"this username is already used: {userCreate.Username}");

            var user = mapper.Map<User>(userCreate);
            user.Role = repository.RoleStandard;

            await repository.AddAsync(user);

            await repository.SaveAsync();

            var userDto = mapper.Map<UserDto>(user);

            return CreatedAtRoute("UserInfo", new { username = userDto.Username }, userDto);
        }

        [HttpPost("~/api/Login"), AllowAnonymous]
        public async Task<ActionResult<String>> Login(UserLoginDto userLogin)
        {
            var userFromRepo = await repository.GetByUsernameAsync(userLogin.Username);

            if (userFromRepo == null ||
                !string.Equals(userLogin.Password, userFromRepo.Password))
            {
                return BadRequest("Username or Password are incorrect");
            }

            var token = jwtAuthenticationManager.CreateToken(userLogin.Username);
            return Ok(token);

        }
        [HttpPut("@{username}")]
        public async Task<ActionResult<string>> UpdateUser(string username, [FromBody] UserUpdateDto userUpdate)
        {
            var userFromRepo = await repository.GetByUsernameAsync(username);

            if (userFromRepo == null) return NotFound();

            var loggedInUsername = User.Identity.Name;
            var loggedInRole = User.FindFirstValue(ClaimTypes.Role);

            if (loggedInRole == "Admin" || string.Equals(loggedInUsername, username, StringComparison.OrdinalIgnoreCase))
            {
                userFromRepo.Email = !String.IsNullOrEmpty(userUpdate.Email) ? userUpdate.Email : userFromRepo.Email;
                userFromRepo.FirstName = !String.IsNullOrEmpty(userUpdate.FirstName) ? userUpdate.FirstName : userFromRepo.FirstName;
                userFromRepo.LastName = !String.IsNullOrEmpty(userUpdate.LastName) ? userUpdate.LastName : userFromRepo.LastName;
                userFromRepo.Password = !String.IsNullOrEmpty(userUpdate.Password) ? userUpdate.Password : userFromRepo.Password;

                repository.Update(userFromRepo);
                await repository.SaveAsync();

                var token = jwtAuthenticationManager.CreateToken(username);
                return Ok(token);

            }
            return BadRequest("You are not allowed to do this action");
        }

    }
}
