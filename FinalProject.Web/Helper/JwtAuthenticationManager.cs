using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using FinalProject.Data.Repositories;
using FinalProject.Data.Models;

namespace FinalProject.Web.Helper;
public class JwtAuthenticationManager
{
    private readonly string key;
    private readonly IUserRepository repository;
    private readonly IConfiguration configuration;

    public JwtAuthenticationManager(IUserRepository repository, IConfiguration configuration)
    {
        this.repository = repository;
        this.configuration = configuration;
        key = configuration.GetSection("Jwt:Key").Value;
    }

    public string CreateToken(UserDto user)
    {
        var timeToLive = configuration.GetValue<int>("Jwt:TokenHours");

        var tokenKey = Encoding.UTF8.GetBytes(key);
        
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject =  new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(timeToLive),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha512Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
