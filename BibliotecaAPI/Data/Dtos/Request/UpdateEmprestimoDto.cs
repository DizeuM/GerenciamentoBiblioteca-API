using BibliotecaAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class UpdateEmprestimoDto
{
    [Required]
    public EmprestimoStatus Status { get; set; }
}
