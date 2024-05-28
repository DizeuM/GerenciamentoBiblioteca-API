using BibliotecaAPI.Data.Dtos.Request;

namespace BibliotecaAPI.Interfaces;

public interface IAuthService
{
    Task<int> AuthenticateUser(AuthDto login);
    Task<string> GenerateToken(int id);
}
