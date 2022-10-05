namespace FinalProject.Web.Helper;

public interface IJwtAuthenticationManager
{
    public string CreateToken(string username);
}