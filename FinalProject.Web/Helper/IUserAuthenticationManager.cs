using FinalProject.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Web.Helper;

public interface IUserAuthenticationManager
{
    Task<IdentityResult> RegisterUserAsync(UserCreateDto userForRegistration);
    Task<bool> ValidateUserAsync(UserLoginDto loginDto);
    Task<string> CreateTokenAsync();
}