using AutoMapper;
using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Enums;
using BibliotecaAPI.Interfaces;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;
    private readonly IUsuarioService _usuarioService;

    public EmprestimoService(BibliotecaContext context, IMapper mapper, IUsuarioService usuarioService)
    {
        _context = context;
        _mapper = mapper;
        _usuarioService = usuarioService;
    }

    public async Task<Emprestimo> GetEmprestimoByIdOrThrowError(int id)
    {
        var emprestimo = await _context.Emprestimos.Include(e => e.Renovacoes).Include(e => e.Multa).FirstOrDefaultAsync(e => e.Id == id);

        if (emprestimo == null)
        {
            throw new NotFoundException("Emprestimo não encontrado.");
        }

        return emprestimo;
    }

    public async Task<IEnumerable<ReadEmprestimoDto>> GetEmprestimosUsuario(int usuarioId)
    {
        var usuario = await _usuarioService.GetUsuarioByIdOrThrowError(usuarioId);

        var emprestimos = usuario.Emprestimos;

        return _mapper.Map<List<ReadEmprestimoDto>>(emprestimos);
    }

    public async Task<ReadEmprestimoDto> CreateEmprestimo(CreateEmprestimoDto emprestimoDto, int funcionarioId)
    {
        var usuario = await _usuarioService.GetUsuarioByIdOrThrowError(emprestimoDto.UsuarioId);

        int numMultasUsuario = usuario.Multas.Count(m => m.Status == MultaStatus.Pendente);
        if (numMultasUsuario > 0)
        {
            throw new BadRequestException("Usuário tem multas pendentes.");
        }

        int numEmprestimosUsuario = usuario.Emprestimos.Count(e => e.Status == EmprestimoStatus.EmAndamento || e.Status == EmprestimoStatus.Atrasado);
        if (numEmprestimosUsuario >= 3)
        {
            throw new BadRequestException("Limite de empréstimos do usuário atingido.");
        }

        var exemplarDisponivel = await _context.Exemplares.FirstOrDefaultAsync(e => e.Id == emprestimoDto.ExemplarId && e.Status == ExemplarStatus.Disponivel);
        if (exemplarDisponivel == null)
        {
            throw new BadRequestException("Exemplar não disponível para empréstimo.");
        }

        var emprestimo = new Emprestimo
        {
            UsuarioId = emprestimoDto.UsuarioId,
            ExemplarId = emprestimoDto.ExemplarId,
            FuncionarioId = funcionarioId,
            DataEmprestimo = DateTime.Now,
            DataLimiteInicial = DateTime.Now.Date.AddDays(7).AddHours(23).AddMinutes(59).AddSeconds(59),
            Status = EmprestimoStatus.EmAndamento
        };

        await _context.Emprestimos.AddAsync(emprestimo);

        var exemplar = await _context.Exemplares.FirstOrDefaultAsync(e => e.Id == emprestimoDto.ExemplarId);
        exemplar.Status = ExemplarStatus.Emprestado;

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
        var emprestimo = await GetEmprestimoByIdOrThrowError(id);

        return _mapper.Map<ReadEmprestimoDto>(emprestimo);
    }

    public async Task ReturnEmprestimo(int id)
    {
        var emprestimo = await GetEmprestimoByIdOrThrowError(id);

        if (emprestimo.Status == EmprestimoStatus.Devolvido)
        {
            throw new BadRequestException("Emprestimo já foi entregue.");
        }

        var exemplar = await _context.Exemplares.FirstOrDefaultAsync(e => e.Id == emprestimo.ExemplarId);
        exemplar.Status = ExemplarStatus.Disponivel;

        emprestimo.DataDevolucao = DateTime.Now;

        if (emprestimo.Status == EmprestimoStatus.Atrasado)
        {
            emprestimo.Status = EmprestimoStatus.Devolvido;
            await _context.SaveChangesAsync();

            throw new Exception("Multa de pendente.");
        }

        emprestimo.Status = EmprestimoStatus.Devolvido;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmprestimosAtrasados()
    {
        var emprestimos = await _context.Emprestimos
            .Include(e => e.Renovacoes)
            .Where(e => e.Status == EmprestimoStatus.EmAndamento || e.Status == EmprestimoStatus.Renovado)
            .ToListAsync();

        foreach (var emprestimo in emprestimos)
        {
            if (emprestimo.Status == EmprestimoStatus.EmAndamento && DateTime.Now > emprestimo.DataLimiteInicial)
            {
                emprestimo.Status = EmprestimoStatus.Atrasado;
                continue;
            }

            var renovacaoAtiva = emprestimo.Renovacoes.FirstOrDefault(r => r.Status == RenovacaoStatus.Ativo);

            if (renovacaoAtiva != null && DateTime.Now > renovacaoAtiva.DataLimiteNova)
            {
                renovacaoAtiva.Status = RenovacaoStatus.Expirado;
                emprestimo.Status = EmprestimoStatus.Atrasado;
            }
        }

        await _context.SaveChangesAsync();
    }

}
