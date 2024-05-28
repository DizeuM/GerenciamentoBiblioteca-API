using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Models;

public class Usuario
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

    public virtual ICollection<Emprestimo> Emprestimos { get; set; }
    public virtual ICollection<Multa> Multas { get; set; }
}
