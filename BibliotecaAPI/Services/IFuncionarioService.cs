using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using Microsoft.AspNetCore.JsonPatch;

public interface IFuncionarioService
{
    ReadFuncionarioDto CreateFuncionario(CreateFuncionarioDto funcionarioDto);
    IEnumerable<ReadFuncionarioDto> GetAllFuncionarios();
    ReadFuncionarioDto GetFuncionarioDto(int id);
    void UpdateFuncionario(int id, UpdateFuncionarioDto funcionarioDto);
    void DeleteFuncionario(int id);
}