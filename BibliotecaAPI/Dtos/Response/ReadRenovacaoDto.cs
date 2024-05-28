using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Dtos.Response;

public class ReadRenovacaoDto
{
    public int Id { get; set; }
    public int EmprestimoId { get; set; }
    public DateTime DataRenovacao { get; set; }
    public DateTime DataLimiteNova { get; set; }
    public RenovacaoStatus Status { get; set; }
}
