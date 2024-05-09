using AutoMapper;
using BibliotecaAPI;
using BibliotecaAPI.Data;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
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

    public async Task<ReadUsuarioDto> CreateUsuario(CreateUsuarioDto usuarioDto)
    {
        var usuario = _mapper.Map<Usuario>(usuarioDto);

        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadUsuarioDto>(usuario);
    }

    public async Task<IEnumerable<ReadUsuarioDto>> GetAllUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();

        var usuariosDto = usuarios.Select(usuario =>
        {
            var emprestimosUsuario = _context.Emprestimos.Where(e => e.UsuarioId == usuario.Id && (e.Status == 1 || e.Status == 3)).Select(e => e.Id).ToList(); ;

            var usuarioDto = _mapper.Map<ReadUsuarioDto>(usuario);

            usuarioDto.Emprestimos = emprestimosUsuario;

            return _mapper.Map<ReadUsuarioDto>(usuarioDto);

        }).ToList();

        return _mapper.Map<List<ReadUsuarioDto>>(usuariosDto);
    }

    public async Task<ReadUsuarioDto> GetUsuarioById(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(f => f.Id == id);
        if (usuario == null) { return null; }


        var emprestimosUsuario = await _context.Emprestimos.Where(e => e.UsuarioId == usuario.Id && (e.Status == 1 || e.Status == 3)).Select(e => e.Id).ToListAsync(); ;

        var usuarioDto = _mapper.Map<ReadUsuarioDto>(usuario);

        usuarioDto.Emprestimos = emprestimosUsuario;

        return usuarioDto;
    }

    public async Task UpdateUsuario(int id, UpdateUsuarioDto usuarioDto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(f => f.Id == id);
        if (usuario == null)
        {
            throw new KeyNotFoundException("Usuario não encontrado");
        }

        _mapper.Map(usuarioDto, usuario);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if (usuario == null)
        {
            throw new KeyNotFoundException("Usuario não encontrado");
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }
}