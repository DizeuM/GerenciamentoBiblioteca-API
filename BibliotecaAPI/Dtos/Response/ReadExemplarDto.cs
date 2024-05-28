using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Data.Dtos.Response;

public class ReadExemplarDto
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public ExemplarStatus Status { get; set; }
}