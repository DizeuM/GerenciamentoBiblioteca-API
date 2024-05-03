using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class CreateEmprestimoDto
{
    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public int ExemplarId { get; set; }
}
