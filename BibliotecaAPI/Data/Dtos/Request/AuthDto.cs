using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class AuthDto
{
    [Required]
    [StringLength(11)]
    public string Cpf { get; set; }

    [Required]
    [MinLength(6)]
    public string Senha { get; set; }
}
