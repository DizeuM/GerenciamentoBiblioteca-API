using AutoMapper;
using BibliotecaAPI.Data;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data.Models;
using BibliotecaAPI.Enums;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ExemplarService : IExemplarService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;
    private readonly ILivroService _livroService;

    public ExemplarService(BibliotecaContext context, IMapper mapper, ILivroService livroService)
    {
        _context = context;
        _mapper = mapper;
        _livroService = livroService;
    }

    public async Task<Exemplar> GetExemplarByIdOrThrowError(int id)
    {
        var exemplar = await _context.Exemplares.Include(e => e.Livro).FirstOrDefaultAsync(e => e.Id == id);
        if (exemplar == null)
        {
            throw new NotFoundException("Exemplar não encontrado.");
        }

        return exemplar;
    }

    public async Task<IEnumerable<ReadExemplarDto>> GetLivroExemplares(int livroId)
    {
        var livro = await _livroService.GetLivroByIdOrThrowError(livroId);

        var exemplares = livro.Exemplares;

        return _mapper.Map<List<ReadExemplarDto>>(exemplares);
    }

    public async Task<ReadExemplarDto> CreateExemplar(CreateExemplarDto exemplarDto)
    {
        var livro = await _livroService.GetLivroByIdOrThrowError(exemplarDto.LivroId);

        var exemplar = new Exemplar
        {
            LivroId = exemplarDto.LivroId,
            Status = ExemplarStatus.Disponivel
        };

        await _context.Exemplares.AddAsync(exemplar);

        await _context.SaveChangesAsync();

        return _mapper.Map<ReadExemplarDto>(exemplar);
    }

    public async Task<IEnumerable<ReadExemplarDto>> GetAllExemplares()
    {
        var exemplares = await _context.Exemplares.ToListAsync();

        return _mapper.Map<List<ReadExemplarDto>>(exemplares);
    }

    public async Task<ReadExemplarDto> GetExemplarById(int id)
    {
        var exemplar = await GetExemplarByIdOrThrowError(id);

        return _mapper.Map<ReadExemplarDto>(exemplar);
    }

    public async Task UpdateExemplar(int id)
    {
        var exemplar = await GetExemplarByIdOrThrowError(id);

        if (exemplar.Status == ExemplarStatus.Emprestado)
        {
            throw new BadRequestException("Exemplar está em uso.");
        }

        if (exemplar.Status == ExemplarStatus.Consulta)
        {
            exemplar.Status = ExemplarStatus.Disponivel;
        }

        if (exemplar.Status == ExemplarStatus.Disponivel)
        {
            exemplar.Status = ExemplarStatus.Consulta;
        }

        await _context.SaveChangesAsync();
    }
}