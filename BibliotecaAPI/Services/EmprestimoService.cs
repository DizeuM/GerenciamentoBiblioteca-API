using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;
    private readonly IUsuarioService _usuarioService;
    private readonly IMultaService _multaService;

    public EmprestimoService(BibliotecaContext context, IMapper mapper, IUsuarioService usuarioService, IMultaService multaService)
    {
        _context = context;
        _mapper = mapper;
        _usuarioService = usuarioService;
        _multaService = multaService;
    }

    public async Task<Emprestimo> GetEmprestimoByIdOrThrowError(int id)
    {
        var emprestimo = await _context.Emprestimos.FirstOrDefaultAsync(e => e.Id == id);
        if (emprestimo == null)
        {
            throw new NotFoundException("Emprestimo não encontrado.");
        }

        return emprestimo;
    }

    public async Task<ReadEmprestimoDto> CreateEmprestimo(CreateEmprestimoDto emprestimoDto, int funcionarioId)
    {
        var usuario = await _usuarioService.GetUsuarioByIdOrThrowError(emprestimoDto.UsuarioId);

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

        var emprestimo = _mapper.Map<Emprestimo>(emprestimoDto);

        emprestimo.FuncionarioId = funcionarioId;
        emprestimo.Status = EmprestimoStatus.EmAndamento;
        emprestimo.DataEmprestimo = DateTime.Now;
        emprestimo.DataPrevistaInicial = DateTime.Now.Date.AddDays(7).AddHours(23).AddMinutes(59).AddSeconds(59);

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
        var emprestimos = await _context.Emprestimos.ToListAsync();

        foreach (var emprestimo in emprestimos)
        {
            if (emprestimo.Status == EmprestimoStatus.EmAndamento && DateTime.Now > emprestimo.DataPrevistaInicial)
            {
                emprestimo.Status = EmprestimoStatus.Atrasado;
                await _context.SaveChangesAsync();
            }
        }
    }

}
