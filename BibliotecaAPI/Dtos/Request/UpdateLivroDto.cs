using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Dtos.Request;

public class UpdateLivroDto
{
    public string Nome { get; set; }
    public string Autor { get; set; }
    public string Editora { get; set; }
    public string Edicao { get; set; }
    public string Genero { get; set; }
    public int QntPaginas { get; set; }
    public float Valor { get; set; }
    public LivroStatus Status { get; set; }
}
