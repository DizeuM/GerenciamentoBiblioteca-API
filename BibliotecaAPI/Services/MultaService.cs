using AutoMapper;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data;
using BibliotecaAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Enums;
using BibliotecaAPI.Interfaces;

namespace BibliotecaAPI.Services;

public class MultaService : IMultaService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;
    private readonly IEmprestimoService _emprestimoService;
    private readonly IUsuarioService _usuarioService;

    public MultaService(BibliotecaContext context, IMapper mapper, IEmprestimoService emprestimoService, IUsuarioService usuarioService)
    {
        _context = context;
        _mapper = mapper;
        _emprestimoService = emprestimoService;
        _usuarioService = usuarioService;
    }

    public async Task<IEnumerable<ReadMultaDto>> GetMultas()
    {
        var multas = await _context.Multas.ToListAsync();
        return _mapper.Map<List<ReadMultaDto>>(multas);
    }

    public async Task<ReadMultaDto> GetMultaById(int id)
    {
        var multa = await _context.Multas.FirstOrDefaultAsync(m => m.Id == id);

        if (multa == null)
        {
            throw new NotFoundException("Multa não encontrada.");
        }

        return _mapper.Map<ReadMultaDto>(multa);
    }

    public async Task<ReadMultaDto> GetMultaByEmprestimo(int emprestimoId)
    {
        var emprestimo = await _emprestimoService.GetEmprestimoByIdOrThrowError(emprestimoId);

        var multa = emprestimo.Multa;

        if (multa == null)
        {
            throw new Exception("Emprestimo não possui multa.");
        }

        return _mapper.Map<ReadMultaDto>(multa);
    }

    public async Task<IEnumerable<ReadMultaDto>> GetMultasUsuario(int usuarioId)
    {
        var usuario = await _usuarioService.GetUsuarioByIdOrThrowError(usuarioId);

        var multas = usuario.Multas;

        return _mapper.Map<List<ReadMultaDto>>(multas);
    }

    public async Task CreateAndUpdateMultas()
    {
        var today = DateTime.Now.Date;

        var emprestimosAtrasados = await _context.Emprestimos
            .Include(e => e.Exemplar.Livro)
            .Include(e => e.Renovacoes)
            .Where(e => e.Status == EmprestimoStatus.Atrasado)
            .ToListAsync();

        foreach (var emprestimo in emprestimosAtrasados)
        {
            var dataLimite = emprestimo.DataLimiteInicial;
            var renovacao = emprestimo.Renovacoes.LastOrDefault();

            if (renovacao != null)
            {
                dataLimite = renovacao.DataLimiteNova;
            }

            int diasUteisAtrasados = 0;

            for (var dia = dataLimite.AddSeconds(1).Date; dia <= DateTime.Now.Date; dia = dia.AddDays(1))
            {
                if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasUteisAtrasados++;
                }
            }

            float valorMulta = diasUteisAtrasados * 1;

            if (valorMulta > emprestimo.Exemplar.Livro.Valor * 2)
            {
                valorMulta = emprestimo.Exemplar.Livro.Valor * 2;
            }

            var multa = await _context.Multas.FirstOrDefaultAsync(m => m.EmprestimoId == emprestimo.Id);

            if (multa == null)
            {
                var novaMulta = new Multa
                {
                    EmprestimoId = emprestimo.Id,
                    Valor = valorMulta,
                    InicioMulta = dataLimite.AddSeconds(1),
                    Status = MultaStatus.Pendente,
                    UsuarioId = emprestimo.UsuarioId
                };

                _context.Multas.Add(novaMulta);
            }
            else
            {
                multa.Valor = valorMulta;
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task PayMulta(int id)
    {
        var multa = await _context.Multas
            .Include(m => m.Emprestimo.Usuario)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (multa == null)
        {
            throw new NotFoundException("Multa não encontrada.");
        }

        if (multa.Emprestimo.Status == EmprestimoStatus.Devolvido)
        {
            multa.FimMulta = DateTime.Now;
            multa.Status = MultaStatus.Paga;
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new BadRequestException("Emprestimo em aberto.");
        }
    }
}
