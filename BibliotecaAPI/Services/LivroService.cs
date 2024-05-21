using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Services;

public class LivroService : ILivroService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;

    public LivroService(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Livro> GetLivroByIdOrThrowError(int id)
    {
        var livro = await _context.Livros.FirstOrDefaultAsync(livro => livro.Id == id);
        if (livro == null)
        {
            throw new NotFoundException("Livro não encontrado");
        }

        return livro;
    }

    public async Task<ReadLivroDto> CreateLivro(CreateLivroDto livroDto)
    {
        var livro = _mapper.Map<Livro>(livroDto);

        livro.Status = LivroStatus.Ativo;

        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadLivroDto>(livro);
    }

    public async Task<IEnumerable<ReadLivroDto>> GetLivros()
    {
        var livros = await _context.Livros.ToListAsync();

        return _mapper.Map<List<ReadLivroDto>>(_context.Livros);
    }

    public async Task<ReadLivroDto> GetLivroById(int id)
    {
        var livro = await GetLivroByIdOrThrowError(id);

        return _mapper.Map<ReadLivroDto>(livro);
    }

    public async Task UpdateLivro(int id, UpdateLivroDto livroDto)
    {
        var livro = await GetLivroByIdOrThrowError(id);

        _mapper.Map(livroDto, livro);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteLivro(int id)
    {
        var livro = await GetLivroByIdOrThrowError(id);

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();
    }
}
