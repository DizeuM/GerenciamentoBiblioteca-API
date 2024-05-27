using AutoMapper;
using BibliotecaAPI;
using BibliotecaAPI.Data;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Enums;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

public class UsuarioService : IUsuarioService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;

    public UsuarioService(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Usuario> GetUsuarioByIdOrThrowError(int id)
    {
        var usuario = await _context.Usuarios.Include(u => u.Emprestimos).Include(u => u.Multas).FirstOrDefaultAsync(u => u.Id == id);
        if (usuario == null)
        {
            throw new NotFoundException("Usuario não encontrado.");
        }

        return usuario;
    }

    public async Task<ReadUsuarioDto> CreateUsuario(CreateUsuarioDto usuarioDto)
    {
        var usuario = _mapper.Map<Usuario>(usuarioDto);

        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadUsuarioDto>(usuario);
    }

    public async Task<IEnumerable<ReadUsuarioDto>> GetAllUsuarios()
    {
        var usuarios = await _context.Usuarios.Include(u => u.Emprestimos).Include(u => u.Multas).ToListAsync();

        return _mapper.Map<List<ReadUsuarioDto>>(usuarios);
    }

    public async Task<ReadUsuarioDto> GetUsuarioById(int id)
    {
        var usuario = await GetUsuarioByIdOrThrowError(id);

        return _mapper.Map<ReadUsuarioDto>(usuario);
    }

    public async Task UpdateUsuario(int id, UpdateUsuarioDto usuarioDto)
    {
        var usuario = await GetUsuarioByIdOrThrowError(id);

        _mapper.Map(usuarioDto, usuario);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUsuario(int id)
    {
        var usuario = await GetUsuarioByIdOrThrowError(id);

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }
}