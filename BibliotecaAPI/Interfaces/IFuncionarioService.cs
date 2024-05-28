using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;
using BibliotecaAPI.Dtos.Request;

public interface IFuncionarioService
{
    Task<Funcionario> GetFuncionarioByIdOrThrowError(int id);
    Task<ReadFuncionarioDto> CreateFuncionario(CreateFuncionarioDto funcionarioDto);
    Task<IEnumerable<ReadFuncionarioDto>> SearchFuncionarioByAttributes(SearchFuncionarioDto searchFuncionarioDto);
    Task<IEnumerable<ReadFuncionarioDto>> GetAllFuncionarios();
    Task<ReadFuncionarioDto> GetFuncionarioById(int id);
    Task UpdateFuncionario(int id, UpdateFuncionarioDto funcionarioDto);
}