using BibliotecaAPI.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Models;

public class Emprestimo
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public int ExemplarId { get; set; }

    [Required]
    public int FuncionarioId { get; set; }

    [Required]
    public DateTime DataEmprestimo { get; set; }

    [Required]
    public DateTime DataLimiteInicial { get; set; }

    public DateTime? DataDevolucao { get; set; }

    [Required]
    public EmprestimoStatus Status { get; set; }

    public Usuario Usuario { get; set; }
    public Exemplar Exemplar { get; set; }
    public Multa Multa { get; set; }
    public virtual ICollection<Renovacao> Renovacoes { get; set; }
}