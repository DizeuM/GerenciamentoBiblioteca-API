using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Data.Dtos.Response;

public class ReadFuncionarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public FuncionarioStatus Status { get; set; }  
}
