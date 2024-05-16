using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;
    private readonly IMultaService _multaService;

    public EmprestimoService(BibliotecaContext context, IMapper mapper, IMultaService multaService)
    {
        _context = context;
        _mapper = mapper;
        _multaService = multaService;
    }

    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public async Task<ReadEmprestimoDto> CreateEmprestimo(CreateEmprestimoDto emprestimoDto, int funcionarioId)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == emprestimoDto.UsuarioId);
        if (usuario == null)
        {
            throw new NotFoundException("Usuário não encontrado.");
        }

        int numEmprestimosUsuario = await _context.Emprestimos.CountAsync(e => e.UsuarioId == emprestimoDto.UsuarioId && (e.Status == 1 || e.Status == 3));
        if (numEmprestimosUsuario >= 3)
        {
            throw new Exception("Limite de empréstimos do usuário atingido.");
        }

        var exemplarDisponivel = await _context.Exemplares.FirstOrDefaultAsync(e => e.Id == emprestimoDto.ExemplarId && e.Status == 1);
        if (exemplarDisponivel == null)
        {
            throw new Exception("Exemplar não disponível para empréstimo.");
        }

        var emprestimo = _mapper.Map<Emprestimo>(emprestimoDto);

        emprestimo.FuncionarioId = funcionarioId;
        emprestimo.Status = 1;
        emprestimo.DataEmprestimo = DateTime.Now;
        emprestimo.DataPrevistaInicial = DateTime.Now.Date.AddDays(7).AddHours(23).AddMinutes(59).AddSeconds(59);

        await _context.Emprestimos.AddAsync(emprestimo);
        await _context.SaveChangesAsync();

        var exemplar = await _context.Exemplares.FirstOrDefaultAsync(e => e.Id == emprestimoDto.ExemplarId);
        exemplar.Status = 2;
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadEmprestimoDto>(emprestimo);
    }

    public async Task<IEnumerable<ReadEmprestimoDto>> GetEmprestimos()
    {
        var emprestimos = await _context.Emprestimos.ToListAsync();

        return _mapper.Map<List<ReadEmprestimoDto>>(emprestimos);
    }

    public async Task<ReadEmprestimoDto> GetEmprestimoById(int id)
    {
        var emprestimo = await _context.Emprestimos.FirstOrDefaultAsync(e => e.Id == id);
        if (emprestimo == null)
        {
            throw new NotFoundException("Emprestimo não encontrado.");
        }

        return _mapper.Map<ReadEmprestimoDto>(emprestimo);
    }

    public async Task ReturnEmprestimo(int id)
    {
        var emprestimo = await _context.Emprestimos.FirstOrDefaultAsync(e => e.Id == id);
        if (emprestimo == null)
        {
            throw new NotFoundException("Emprestimo não encontrado.");
        }

        if (emprestimo.Status == 2)
        {
            throw new Exception("Emprestimo já foi entregue.");
        }

        if (emprestimo.Status == 3)
        {
            //throw new Exception("Multa pendente");
        }

        emprestimo.Status = 2;
        emprestimo.DataDevolucao = DateTime.Now;
        await _context.SaveChangesAsync();

        var exemplar = _context.Exemplares.FirstOrDefault(e => e.Id == emprestimo.ExemplarId);
        exemplar.Status = 1;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmprestimosAtrasados()
    {
        var emprestimos = await _context.Emprestimos.ToListAsync();

        foreach (var emprestimo in emprestimos)
        {
            if (emprestimo.Status == 1 && DateTime.Now > emprestimo.DataPrevistaInicial)
            {
                emprestimo.Status = 3;
                await _context.SaveChangesAsync();
            }
        }
    }

}
