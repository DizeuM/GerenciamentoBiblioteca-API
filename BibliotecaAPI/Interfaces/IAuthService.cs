using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Interfaces;

public interface IAuthService
{
    Task<int> AuthenticateUser(AuthDto login);
    Task<string> GenerateToken(int id);
}
