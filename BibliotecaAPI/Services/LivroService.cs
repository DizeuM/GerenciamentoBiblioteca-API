using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Enums;
using BibliotecaAPI.Interfaces;

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
        var livro = await _context.Livros.Include(e => e.Exemplares).FirstOrDefaultAsync(livro => livro.Id == id);
        if (livro == null)
        {
            throw new NotFoundException("Livro não encontrado.");
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

    public async Task<IEnumerable<ReadLivroDto>> SearchLivroByAttributes(SearchLivroDto livroDto)
    {
        var query = _context.Livros.Include(e => e.Exemplares).AsQueryable();

        if (!string.IsNullOrWhiteSpace(livroDto.Nome))
        {
            query = query.Where(l => l.Nome.ToLower().Contains(livroDto.Nome.ToLower().Trim()));
        }

        if (!string.IsNullOrWhiteSpace(livroDto.Autor))
        {
            query = query.Where(l => l.Autor.ToLower().Contains(livroDto.Autor.ToLower().Trim()));
        }

        if (!string.IsNullOrWhiteSpace(livroDto.Editora))
        {
            query = query.Where(l => l.Editora.ToLower().Contains(livroDto.Editora.ToLower().Trim()));
        }

        if (!string.IsNullOrWhiteSpace(livroDto.Edicao))
        {
            query = query.Where(l => l.Edicao.ToLower().Contains(livroDto.Edicao.ToLower().Trim()));
        }

        if (!string.IsNullOrWhiteSpace(livroDto.Genero))
        {
            query = query.Where(l => l.Genero.ToLower() == livroDto.Genero.ToLower().Trim());
        }

        if (livroDto.Status != null)
        {
            query = query.Where(l => l.Status == livroDto.Status);
        }

        var livros = await query.ToListAsync();

        var livrosDto = livros.Select(livro =>
        {
            var livroDto = _mapper.Map<ReadLivroDto>(livro);

            var qntExemplarDisponivel = livro.Exemplares.Where(e => e.Status == ExemplarStatus.Disponivel).Count();
            livroDto.QntExemplarDisponivel = qntExemplarDisponivel;

            return livroDto;

        }).ToList();

        return livrosDto;
    }

    public async Task<IEnumerable<ReadLivroDto>> GetLivros()
    {
        var livros = await _context.Livros.Include(e => e.Exemplares).ToListAsync();

        var livrosDto = livros.Select(livro =>
        {
            var livroDto = _mapper.Map<ReadLivroDto>(livro);

            var qntExemplarDisponivel = livro.Exemplares.Where(e => e.Status == ExemplarStatus.Disponivel).Count();
            livroDto.QntExemplarDisponivel = qntExemplarDisponivel;

            return livroDto;

        }).ToList();

        return livrosDto;
    }

    public async Task<ReadLivroDto> GetLivroById(int id)
    {
        var livro = await GetLivroByIdOrThrowError(id);

        var livroDto = _mapper.Map<ReadLivroDto>(livro);

        var qntExemplarDisponivel = livro.Exemplares.Where(e => e.Status == ExemplarStatus.Disponivel).Count();
        livroDto.QntExemplarDisponivel = qntExemplarDisponivel;

        return livroDto;
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
