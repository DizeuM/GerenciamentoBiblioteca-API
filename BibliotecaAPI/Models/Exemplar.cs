using BibliotecaAPI.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Exemplar
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int LivroId { get; set; }

    [Required]
    public ExemplarStatus Status { get; set; }

    [JsonIgnore]
    public Livro Livro { get; set; }

    public virtual ICollection<Emprestimo> Emprestimos { get; set; }
}