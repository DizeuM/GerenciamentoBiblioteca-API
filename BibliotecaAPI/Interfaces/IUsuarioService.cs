using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data.Models;

public interface IUsuarioService
{
    Task<Usuario> GetUsuarioByIdOrThrowError(int id);
    Task<ReadUsuarioDto> CreateUsuario(CreateUsuarioDto usuarioDto);
    Task<IEnumerable<ReadUsuarioDto>> GetAllUsuarios();
    Task<ReadUsuarioDto> GetUsuarioById(int id);
    Task UpdateUsuario(int id, UpdateUsuarioDto usuarioDto);
    Task DeleteUsuario(int id);
}