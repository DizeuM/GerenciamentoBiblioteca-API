using BibliotecaAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Models;

public class Funcionario
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string Cpf { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string Telefone { get; set; }

    [Required]
    [MinLength(6)]
    public string Senha { get; set; }

    [Required]
    public FuncionarioStatus Status { get; set; }
}
