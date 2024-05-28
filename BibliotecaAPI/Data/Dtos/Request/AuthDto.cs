using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class AuthDto
{
    public string Cpf { get; set; }
    public string Senha { get; set; }
}
