using AutoMapper;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Services;

public class MultaService : IMultaService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;

    public MultaService(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadMultaDto>> GetMultas()
    {
        var multas = await _context.Multas.ToListAsync();
        return _mapper.Map<List<ReadMultaDto>>(multas);
    }

    public async Task<ReadMultaDto> GetMultaById(int id)
    {
        var multa = await _context.Multas.FirstOrDefaultAsync(e => e.Id == id);
        if (multa == null)
        {
            throw new NotFoundException("Multa não encontrada.");
        }

        return _mapper.Map<ReadMultaDto>(multa);
    }

    public async Task CreateAndUpdateMultas()
    {
        var emprestimosAtrasados = await _context.Emprestimos.Include(e => e.Exemplar.Livro).Where(e => e.Status == EmprestimoStatus.Atrasado).ToListAsync();

        foreach (var emprestimo in emprestimosAtrasados)
        {
            int diasUteisAtrasados = 0;

            for (var dia = emprestimo.DataPrevistaInicial.AddSeconds(1).Date; dia <= DateTime.Now.Date; dia = dia.AddDays(1))
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

            var multa = _context.Multas.FirstOrDefault(e => e.EmprestimoId == emprestimo.Id);

            if (multa == null)
            {
                Multa novaMulta = new Multa();

                novaMulta.EmprestimoId = emprestimo.Id;
                novaMulta.Valor = valorMulta;
                novaMulta.InicioMulta = emprestimo.DataPrevistaInicial.AddSeconds(1);
                novaMulta.Status = MultaStatus.Pendente;
                novaMulta.UsuarioId = emprestimo.UsuarioId;

                _context.Multas.Add(novaMulta);
            }
            else
            {
                multa.Valor = valorMulta;
            }

            _context.SaveChanges();
        }
    }

    public async Task PagarMulta(int id)
    {
        var multa = await _context.Multas.Include(m => m.Emprestimo.Usuario).FirstOrDefaultAsync(e => e.Id == id);
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
