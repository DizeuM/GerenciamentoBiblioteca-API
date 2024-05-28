using BibliotecaAPI.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Models;

public class Livro
{

    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [Required]
    [MaxLength(100)]
    public string Autor { get; set; }

    [Required]
    [MaxLength(100)]
    public string Editora { get; set; }

    [Required]
    [MaxLength(50)]
    public string Edicao { get; set; }

    [Required]
    [MaxLength(50)]
    public string Genero { get; set; }

    [Required]
    public int QntPaginas { get; set; }

    [Required]
    public float Valor { get; set; }

    [Required]
    public LivroStatus Status { get; set; }

    public virtual ICollection<Exemplar> Exemplares { get; set; }
}
