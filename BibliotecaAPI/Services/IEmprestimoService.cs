using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;

namespace BibliotecaAPI.Services;

public interface IEmprestimoService
{
    Task<ReadEmprestimoDto> CreateEmprestimo(CreateEmprestimoDto emprestimoDto, int funcionarioId);
    Task<IEnumerable<ReadEmprestimoDto>> GetEmprestimos();
    Task<ReadEmprestimoDto> GetEmprestimoById(int id);
    Task UpdateEmprestimo(int id);
}
