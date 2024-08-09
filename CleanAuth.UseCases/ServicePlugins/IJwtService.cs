using CleanAuth.CoreBusiness.Entities;

namespace CleanAuth.UseCases.ServicesPlugins
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string ValidateToken(string token);
    }
}
