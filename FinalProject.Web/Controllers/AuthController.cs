using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;
using FinalProject.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FinalProject.Web.Controllers
{
    [Route("api")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IUserAuthenticationManager authenticationManager;

        public AuthController(UserManager<User> userManager,
            IMapper mapper,
            IUserAuthenticationManager authenticationManager)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.authenticationManager = authenticationManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(UserCreateDto userCreate)
        {
            var userResult = await authenticationManager.RegisterUserAsync(userCreate);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : new CreatedResult("UserInfo", userCreate);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserLoginDto userLogin)
        {
            return !await authenticationManager.ValidateUserAsync(userLogin)
            ? Unauthorized()
            : Ok(new { Token = authenticationManager.CreateToken() });

        }

    }
}
