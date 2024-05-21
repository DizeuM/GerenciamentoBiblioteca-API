using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;

public interface IFuncionarioService
{
    Task<Funcionario> GetFuncionarioByIdOrThrowError(int id);
    Task<ReadFuncionarioDto> CreateFuncionario(CreateFuncionarioDto funcionarioDto);
    Task<IEnumerable<ReadFuncionarioDto>> GetAllFuncionarios();
    Task<ReadFuncionarioDto> GetFuncionarioById(int id);
    Task UpdateFuncionario(int id, UpdateFuncionarioDto funcionarioDto);
}