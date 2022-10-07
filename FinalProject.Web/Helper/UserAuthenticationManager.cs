using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using FinalProject.Data.Entities;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using FinalProject.Data.Models;

namespace FinalProject.Web.Helper;
public class UserAuthenticationManager : IUserAuthenticationManager
{
    private User _user;
    private readonly IConfiguration configuration;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public UserAuthenticationManager(IConfiguration configuration, UserManager<User> userManager, IMapper mapper)
    {
        this.configuration = configuration;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserCreateDto userRegistration)
    {
        var user = mapper.Map<User>(userRegistration);
        var result = await userManager.CreateAsync(user, userRegistration.Password);
        return result;
    }

    public async Task<bool> ValidateUserAsync(UserLoginDto loginDto)
    {
        _user = await userManager.FindByNameAsync(loginDto.UserName);
        var result = _user != null && await userManager.CheckPasswordAsync(_user, loginDto.Password);
        return result;
    }

    public string CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var jwtConfig = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private List<Claim> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };
        
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var tokenOptions = new JwtSecurityToken
        (
        issuer: jwtSettings["validIssuer"],
        audience: jwtSettings["validAudience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
        signingCredentials: signingCredentials
        );
        return tokenOptions;
    }
}
