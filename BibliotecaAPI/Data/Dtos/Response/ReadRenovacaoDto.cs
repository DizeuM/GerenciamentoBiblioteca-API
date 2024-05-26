using BibliotecaAPI.Enums;
using BibliotecaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Response;

public class ReadRenovacaoDto
{
    public int Id { get; set; }
    public int EmprestimoId { get; set; }
    public DateTime DataRenovacao { get; set; }
    public DateTime DataLimiteNova { get; set; }
    public RenovacaoStatus Status { get; set; }
}
