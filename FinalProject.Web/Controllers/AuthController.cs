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

        private readonly IUserAuthenticationManager authenticationManager;

        public AuthController(IUserAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(UserCreateDto userCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userResult = await authenticationManager.RegisterUserAsync(userCreate);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : new CreatedResult("UserInfo", userCreate);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserLoginDto userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return !await authenticationManager.ValidateUserAsync(userLogin)
            ? Unauthorized()
            : Ok(new { Token = authenticationManager.CreateToken() });

        }

    }
}
