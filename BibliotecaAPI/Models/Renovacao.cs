using BibliotecaAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Renovacao
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int EmprestimoId { get; set; }

    [Required]
    public DateTime DataRenovacao { get; set; }

    [Required]
    public DateTime DataLimiteNova { get; set; }

    [Required]
    public RenovacaoStatus Status { get; set; }

    public Emprestimo Emprestimo { get; set; }

}