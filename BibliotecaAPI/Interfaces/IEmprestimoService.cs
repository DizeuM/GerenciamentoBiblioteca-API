using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Interfaces;

public interface IEmprestimoService
{
    Task<Emprestimo> GetEmprestimoByIdOrThrowError(int id);
    Task<IEnumerable<ReadEmprestimoDto>> GetEmprestimosUsuario(int usuarioId);
    Task<ReadEmprestimoDto> CreateEmprestimo(CreateEmprestimoDto emprestimoDto, int funcionarioId);
    Task<IEnumerable<ReadEmprestimoDto>> GetEmprestimos();
    Task<ReadEmprestimoDto> GetEmprestimoById(int id);
    Task ReturnEmprestimo(int id);
    Task UpdateEmprestimosAtrasados();
}
