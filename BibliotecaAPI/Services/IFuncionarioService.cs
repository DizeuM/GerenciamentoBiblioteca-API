using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;

public interface IFuncionarioService
{
    Task<ReadFuncionarioDto> CreateFuncionario(CreateFuncionarioDto funcionarioDto);
    Task<IEnumerable<ReadFuncionarioDto>> GetAllFuncionarios();
    Task<ReadFuncionarioDto> GetFuncionarioDto(int id);
    Task UpdateFuncionario(int id, UpdateFuncionarioDto funcionarioDto);
}