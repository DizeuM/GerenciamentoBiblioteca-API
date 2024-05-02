using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ReadLivroDto> CreateLivro(CreateLivroDto livroDto)
    {
        var livro = _mapper.Map<Livro>(livroDto);

        livro.Status = 1;
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
        var livro = await _context.Livros.FirstOrDefaultAsync(livro => livro.Id == id);
        if (livro == null){return null;}

        var livroDto = _mapper.Map<ReadLivroDto>(livro);

        return livroDto;
    }

    public async Task UpdateLivro(int id, UpdateLivroDto livroDto)
    {
        var livro = await _context.Livros.FirstOrDefaultAsync(livro => livro.Id == id);

        if (livro == null)
        {
            throw new KeyNotFoundException("Livro não encontrado.");
        }

        _mapper.Map(livroDto, livro);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLivro(int id)
    {
        var livro = await _context.Livros.FirstOrDefaultAsync(livro => livro.Id == id);

        if (livro == null)
        {
            throw new KeyNotFoundException("Livro não encontrado.");
        }

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();
    }
}
