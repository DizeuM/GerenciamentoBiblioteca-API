using BibliotecaAPI.Data;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;
using BibliotecaAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BibliotecaAPI.Services;

public class AuthService : IAuthService
{
    private BibliotecaContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(BibliotecaContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<int> AuthenticateUser(AuthDto login)
    {
        var hashSenha = BCrypt.Net.BCrypt.HashPassword(login.Senha, _configuration["PasswordHash:Salt"]);
        var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Cpf == login.Cpf && f.Senha == hashSenha);

        if (funcionario == null)
        {
            throw new UnauthorizedException("CPF ou Senha incorretos.");
        }

        return funcionario.Id;
    }

    public async Task<string> GenerateToken(int id)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] { new Claim("FuncionarioId", id.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        var funcionarioToken = handler.WriteToken(token);

        return funcionarioToken;
    }
}
