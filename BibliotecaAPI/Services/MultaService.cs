using AutoMapper;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

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

    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
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
        var emprestimosAtrasados = await _context.Emprestimos.Include(e => e.Exemplar.Livro).Where(e => e.Status == 3).ToListAsync();

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
                novaMulta.Status = 1;

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
        var multa = _context.Multas.Include(m => m.Emprestimo.Usuario).FirstOrDefault(e => e.Id == id);
        if (multa == null) 
        {
            throw new NotFoundException("Multa não encontrada.");
        }

        if (multa.Emprestimo.Status == 2)
        {
            multa.FimMulta = DateTime.Now;
            multa.Status = 2;
            _context.SaveChanges();
        }
        
        else
        {
            throw new Exception("Emprestimo em aberto.");
        }
    }

}
