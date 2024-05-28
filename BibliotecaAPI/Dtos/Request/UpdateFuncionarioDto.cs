using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Data.Dtos.Request;

public class UpdateFuncionarioDto
{
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Senha { get; set; }
    public FuncionarioStatus Status { get; set; }
}
