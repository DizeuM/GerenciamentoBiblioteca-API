using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class CreateLivroDto
{
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
}
