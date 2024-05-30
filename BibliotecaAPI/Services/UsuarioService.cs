using AutoMapper;
using BibliotecaAPI.Data;
using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;
using BibliotecaAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Enums;

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

    public async Task UsuarioHasNoIssues(int id)
    {
        var usuario = await GetUsuarioByIdOrThrowError(id);

        if (usuario.Multas.Any(m => m.Status == MultaStatus.Pendente))
        {
            throw new BadRequestException("Usuário tem multas pendentes.");
        }

        int numEmprestimosUsuario = usuario.Emprestimos.Count(e => e.Status != EmprestimoStatus.Devolvido);
        if (numEmprestimosUsuario >= 3)
        {
            throw new BadRequestException("Limite de empréstimos do usuário atingido.");
        }
    }

    public async Task<ReadUsuarioDto> CreateUsuario(CreateUsuarioDto usuarioDto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Cpf == usuarioDto.Cpf))
        {
            throw new BadRequestException("Já existe um usuário cadastrado com esse CPF.");
        }

        var usuario = _mapper.Map<Usuario>(usuarioDto);

        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadUsuarioDto>(usuario);
    }

    public async Task<IEnumerable<ReadUsuarioDto>> SearchUsuarioByAttributes(SearchUsuarioDto searchUsuarioDto)
    {
        var query = _context.Usuarios.Include(u => u.Emprestimos).Include(u => u.Multas).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchUsuarioDto.Nome))
        {
            query = query.Where(f => f.Nome.ToLower().Contains(searchUsuarioDto.Nome.ToLower().Trim()));
        }

        if (!string.IsNullOrWhiteSpace(searchUsuarioDto.Cpf))
        {
            query = query.Where(f => f.Cpf == searchUsuarioDto.Cpf);
        }

        if (!string.IsNullOrWhiteSpace(searchUsuarioDto.Email))
        {
            query = query.Where(f => f.Email.ToLower().Contains(searchUsuarioDto.Email.ToLower().Trim()));
        }

        var usuarios = await query.ToListAsync();

        return _mapper.Map<List<ReadUsuarioDto>>(usuarios);
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