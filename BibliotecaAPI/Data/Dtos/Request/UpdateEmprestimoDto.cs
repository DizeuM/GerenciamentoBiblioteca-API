using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class UpdateEmprestimoDto
{
    [Required]
    public int Status { get; set; }
}
