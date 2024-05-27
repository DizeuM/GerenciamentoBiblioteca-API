using AutoMapper;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Enums;
using BibliotecaAPI.Interfaces;

namespace BibliotecaAPI.Services;

public class RenovacaoService : IRenovacaoService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;
    private readonly IEmprestimoService _emprestimoService;

    public RenovacaoService(BibliotecaContext context, IMapper mapper, IEmprestimoService emprestimoService)
    {
        _context = context;
        _mapper = mapper;
        _emprestimoService = emprestimoService;
    }

    public async Task<Renovacao> GetRenovacaoByIdOrThrowError(int id)
    {
        var renovacao = await _context.Renovacoes.FirstOrDefaultAsync(r => r.Id == id);

        if (renovacao == null)
        {
            throw new NotFoundException("Renovação não encontrada.");
        }

        return renovacao;
    }

    public async Task<IEnumerable<ReadRenovacaoDto>> GetRenovacoes()
    {
        var renovacoes = await _context.Renovacoes.ToListAsync();

        return _mapper.Map<List<ReadRenovacaoDto>>(renovacoes);
    }

    public async Task<ReadRenovacaoDto> GetRenovacaoById(int id)
    {
        var renovacao = await GetRenovacaoByIdOrThrowError(id);

        return _mapper.Map<ReadRenovacaoDto>(renovacao);
    }

    public async Task<ReadRenovacaoDto> CreateRenovacao(int emprestimoId)
    {
        var emprestimo = await _emprestimoService.GetEmprestimoByIdOrThrowError(emprestimoId);

        if (emprestimo.Status != EmprestimoStatus.EmAndamento && emprestimo.Status != EmprestimoStatus.Renovado)
        {
            throw new BadRequestException("Emprestimo não pode ser renovado.");
        }

        int numRenovacoes = emprestimo.Renovacoes.Count();

        if (numRenovacoes >= 3)
        {
            throw new BadRequestException("Emprestimo atingiu o limite de renovações.");
        }

        Renovacao novaRenovacao;

        if (numRenovacoes == 0)
        {
            novaRenovacao = new Renovacao
            {
                EmprestimoId = emprestimoId,
                DataRenovacao = DateTime.Now,
                DataLimiteNova = DateTime.Now.Date.AddDays(7).AddHours(23).AddMinutes(59).AddSeconds(59),
                Status = RenovacaoStatus.Ativo
            };

            emprestimo.Status = EmprestimoStatus.Renovado;
        }
        else
        {
            var renovacaoAtiva = emprestimo.Renovacoes.FirstOrDefault(r => r.Status == RenovacaoStatus.Ativo);
            renovacaoAtiva.Status = RenovacaoStatus.Expirado;

            novaRenovacao = new Renovacao
            {
                EmprestimoId = emprestimoId,
                DataRenovacao = DateTime.Now,
                DataLimiteNova = DateTime.Now.Date.AddDays(7).AddHours(23).AddMinutes(59).AddSeconds(59),
                Status = RenovacaoStatus.Ativo
            };
        }

        await _context.Renovacoes.AddAsync(novaRenovacao);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadRenovacaoDto>(novaRenovacao);
    }
}
