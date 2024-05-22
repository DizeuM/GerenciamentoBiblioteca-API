using BibliotecaAPI.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Multa
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int EmprestimoId { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public double Valor { get; set; }

    [Required]
    public DateTime InicioMulta { get; set; }

    public DateTime? FimMulta { get; set; }

    [Required]
    public MultaStatus Status { get; set; }

    [JsonIgnore]
    public Emprestimo Emprestimo { get; set; }

    [JsonIgnore]
    public Usuario Usuario { get; set; }
}
