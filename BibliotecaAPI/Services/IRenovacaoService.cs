using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Services;

public interface IRenovacaoService
{
    Task<Renovacao> GetRenovacaoByIdOrThrowError(int id);
    Task<IEnumerable<ReadRenovacaoDto>> GetRenovacoes();
    Task<ReadRenovacaoDto> GetRenovacaoById(int id);
    Task<ReadRenovacaoDto> CreateRenovacao(int emprestimoId);

}
