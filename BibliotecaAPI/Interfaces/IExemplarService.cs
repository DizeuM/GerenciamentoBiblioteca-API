using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data.Models;

public interface IExemplarService
{
    Task<Exemplar> GetExemplarByIdOrThrowError(int id);
    Task<IEnumerable<ReadExemplarDto>> GetLivroExemplares(int livroId);
    Task<ReadExemplarDto> CreateExemplar(CreateExemplarDto exemplarDto);
    Task<IEnumerable<ReadExemplarDto>> GetAllExemplares();
    Task<ReadExemplarDto> GetExemplarById(int id);
    Task UpdateExemplar(int id);
}