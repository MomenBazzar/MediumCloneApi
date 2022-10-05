using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace FinalProject.Web.Helper;
public class JwtAuthenticationManager : IJwtAuthenticationManager
{
    private readonly string key;
    private readonly IConfiguration configuration;

    public JwtAuthenticationManager(IConfiguration configuration)
    {
        this.configuration = configuration;
        key = configuration.GetSection("Jwt:Key").Value;
    }

    public string CreateToken(string username)
    {
        var timeToLive = configuration.GetValue<int>("Jwt:TokenHours");

        var tokenKey = Encoding.UTF8.GetBytes(key);
        
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject =  new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
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
