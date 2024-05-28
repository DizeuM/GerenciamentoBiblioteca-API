using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class UpdateUsuarioDto
{
    public string Email { get; set; }
    public string Telefone { get; set; }
}
