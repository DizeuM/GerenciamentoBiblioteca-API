using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;
using BibliotecaAPI.Dtos.Request;

public interface IUsuarioService
{
    Task<Usuario> GetUsuarioByIdOrThrowError(int id);
    Task<ReadUsuarioDto> CreateUsuario(CreateUsuarioDto usuarioDto);
    Task<IEnumerable<ReadUsuarioDto>> SearchUsuarioByAttributes(SearchUsuarioDto searchUsuarioDto);
    Task<IEnumerable<ReadUsuarioDto>> GetAllUsuarios();
    Task<ReadUsuarioDto> GetUsuarioById(int id);
    Task UpdateUsuario(int id, UpdateUsuarioDto usuarioDto);
    Task DeleteUsuario(int id);
}