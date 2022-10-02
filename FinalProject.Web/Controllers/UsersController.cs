using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;
using FinalProject.Data.Repositories;
using FinalProject.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;

        public UsersController(IUserRepository repository, IMapper mapper, IConfiguration configuration)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.configuration = configuration;
            this.jwtAuthenticationManager = new JwtAuthenticationManager(repository, configuration);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await repository.GetAllAsync();

            var usersDto = mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(usersDto);
        }

        [HttpGet("@{username}", Name = "UserInfo")]
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

            return CreatedAtRoute("UserInfo", new {username = userDto.Username}, userDto);
        }

        [HttpPost("~/api/Login"), AllowAnonymous]
        public async Task<ActionResult<String>> Login(UserLoginDto userLogin)
        {
            var userFromRepo = await repository.GetByUsernameAsync(userLogin.Username);
            
            if (userFromRepo == null || 
                !string.Equals(userLogin.Password, userFromRepo.Password, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Username or Password are incorrect");
            }

            var user = mapper.Map<UserDto>(userFromRepo);

            var token = jwtAuthenticationManager.CreateToken(user);
            return Ok(token);

        }
        [HttpPut("@{username}")]
        public void Put(string username, [FromBody] UserDto userDto)
        {
            
        }

    }
}
