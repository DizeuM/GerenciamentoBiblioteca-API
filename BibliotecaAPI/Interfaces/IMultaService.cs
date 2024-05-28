using BibliotecaAPI.Data.Dtos.Response;

namespace BibliotecaAPI.Interfaces;

public interface IMultaService
{
    Task<IEnumerable<ReadMultaDto>> GetMultas();
    Task<ReadMultaDto> GetMultaById(int id);
    Task CreateAndUpdateMultas();
    Task PayMulta(int id);
}
