using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

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
    public DateTime DataPrevistaInicial { get; set; }
    public DateTime? DataDevolucao { get; set; }

    [Required]
    public int Status { get; set; }

    [JsonIgnore]
    public Usuario Usuario { get; set; }

    [JsonIgnore]
    public Exemplar Exemplar { get; set; }
}