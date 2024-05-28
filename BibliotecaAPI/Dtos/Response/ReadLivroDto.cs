namespace BibliotecaAPI.Data.Dtos.Response;

public class ReadLivroDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Autor { get; set; }
    public string Editora { get; set; }
    public string Edicao { get; set; }
    public string Genero { get; set; }
    public int QntPaginas { get; set; }
    public float Valor { get; set; }
    public int QntExemplarDisponivel { get; set; }
    public int Status { get; set; }
}