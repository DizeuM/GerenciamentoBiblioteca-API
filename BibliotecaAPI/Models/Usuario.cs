using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Usuario
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

    public virtual ICollection<Emprestimo> Emprestimos { get; set; }

    public virtual ICollection<Multa> Multas { get; set; }
}
