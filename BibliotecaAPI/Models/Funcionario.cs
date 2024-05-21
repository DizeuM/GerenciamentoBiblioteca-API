using BibliotecaAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Funcionario
{
    [Key]
    [Required]
    public int Id { get; set; }

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

    [Required]
    [MinLength(6)]
    public string Senha { get; set; }

    [Required]
    public FuncionarioStatus Status { get; set; }
}
