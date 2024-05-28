using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Dtos.Request;

public class SearchFuncionarioDto
{
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public string? Email { get; set; }
    public FuncionarioStatus? Status { get; set; }
}
