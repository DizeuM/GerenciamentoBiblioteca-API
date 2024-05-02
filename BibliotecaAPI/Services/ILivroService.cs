using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;

namespace BibliotecaAPI.Services;

public interface ILivroService
{
    Task<ReadLivroDto> CreateLivro(CreateLivroDto livroDto);
    Task<IEnumerable<ReadLivroDto>> GetLivros();
    Task<ReadLivroDto> GetLivroById(int id);
    Task UpdateLivro(int id, UpdateLivroDto livroDto);
    Task DeleteLivro(int id);
}
