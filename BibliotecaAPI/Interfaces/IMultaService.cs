using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Interfaces;

public interface IMultaService
{
    Task<IEnumerable<ReadMultaDto>> GetMultas();
    Task<ReadMultaDto> GetMultaById(int id);
    Task CreateAndUpdateMultas();
    Task PayMulta(int id);
}
