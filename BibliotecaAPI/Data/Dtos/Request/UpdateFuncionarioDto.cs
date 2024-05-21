using BibliotecaAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class UpdateFuncionarioDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(11)]
    public string Telefone { get; set; }

    [Required]
    [MinLength(6)]
    public string Senha { get; set; }

    [Required]
    public FuncionarioStatus Status { get; set; }
}
