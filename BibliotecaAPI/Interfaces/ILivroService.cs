using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Interfaces;

public interface ILivroService
{
    Task<Livro> GetLivroByIdOrThrowError(int id);
    Task<ReadLivroDto> CreateLivro(CreateLivroDto livroDto);
    Task<IEnumerable<ReadLivroDto>> SearchLivroByAttributes(SearchLivroDto livroDto);
    Task<IEnumerable<ReadLivroDto>> GetLivros();
    Task<ReadLivroDto> GetLivroById(int id);
    Task UpdateLivro(int id, UpdateLivroDto livroDto);
    Task DeleteLivro(int id);
}
