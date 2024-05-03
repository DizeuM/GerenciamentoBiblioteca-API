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
    public int Status { get; set; }
}