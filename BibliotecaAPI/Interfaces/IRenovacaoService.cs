using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Interfaces;

public interface IRenovacaoService
{
    Task<Renovacao> GetRenovacaoByIdOrThrowError(int id);
    Task<IEnumerable<ReadRenovacaoDto>> GetRenovacoes();
    Task<ReadRenovacaoDto> GetRenovacaoById(int id);
    Task<ReadRenovacaoDto> CreateRenovacao(int emprestimoId);

}
