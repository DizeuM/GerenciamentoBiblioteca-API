using BibliotecaAPI.Data;
using BibliotecaAPI.Data.Dtos.Request;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BibliotecaAPI.Services;

public class AuthService
{
    private BibliotecaContext _context;

    public AuthService(BibliotecaContext context)
    {
        _context = context;
    }

    public string GenerateToken(AuthDto login)
    {
        string hashSenha = BCrypt.Net.BCrypt.HashPassword(login.Senha, Key.Salt);

        var loginFuncionario = _context.Funcionarios.SingleOrDefault(f => f.Cpf == login.Cpf && f.Senha == hashSenha);
        if (loginFuncionario == null){return string.Empty;}

        var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Cpf == login.Cpf);

        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Key.Secret);

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] { new Claim("FuncionarioId", funcionario.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        var funcionarioToken = handler.WriteToken(token);

        return funcionarioToken;
    }
}
