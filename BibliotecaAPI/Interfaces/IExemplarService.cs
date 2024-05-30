using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;

public interface IExemplarService
{
    Task<Exemplar> GetExemplarByIdOrThrowError(int id);
    Task<Exemplar> ExemplarDisponivel(int id);
    Task<IEnumerable<ReadExemplarDto>> GetLivroExemplares(int livroId);
    Task<ReadExemplarDto> CreateExemplar(CreateExemplarDto exemplarDto);
    Task<IEnumerable<ReadExemplarDto>> GetAllExemplares();
    Task<ReadExemplarDto> GetExemplarById(int id);
    Task UpdateExemplar(int id);
}