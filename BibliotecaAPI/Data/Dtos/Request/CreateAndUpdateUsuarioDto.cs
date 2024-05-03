using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class CreateAndUpdateUsuarioDto
{
    [Required]
    public string Nome { get; set; }

    [Required]
    [StringLength(11)]
    public string Cpf { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(11)]
    public string Telefone { get; set; }
}
