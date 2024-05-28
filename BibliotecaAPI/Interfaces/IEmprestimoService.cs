using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Interfaces;

public interface IEmprestimoService
{
    Task<Emprestimo> GetEmprestimoByIdOrThrowError(int id);
    Task<ReadEmprestimoDto> CreateEmprestimo(CreateEmprestimoDto emprestimoDto, int funcionarioId);
    Task<IEnumerable<ReadEmprestimoDto>> GetEmprestimos();
    Task<ReadEmprestimoDto> GetEmprestimoById(int id);
    Task ReturnEmprestimo(int id);
    Task UpdateEmprestimosAtrasados();
}
