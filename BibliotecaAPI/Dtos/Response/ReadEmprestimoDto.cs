using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Data.Dtos.Response;

public class ReadEmprestimoDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int ExemplarId { get; set; }
    public int FuncionarioId { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataPrevistaInicial { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public EmprestimoStatus Status { get; set; }
}
