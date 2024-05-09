using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Livro
{

    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; } 

    [Required]
    public string Autor { get; set; }

    [Required]
    public string Editora { get; set; }

    [Required]
    public string Edicao { get; set; }

    [Required] 
    public string Genero { get; set; }

    [Required]
    public int QntPaginas { get; set; } 

    [Required]
    public float Valor { get; set; }

    [Required]
    public int Status { get; set; }

    [JsonIgnore]
    public virtual ICollection<Exemplar> Exemplares { get; set; }
}
