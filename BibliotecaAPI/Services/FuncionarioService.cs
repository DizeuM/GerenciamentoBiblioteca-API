using AutoMapper;
using BibliotecaAPI;
using BibliotecaAPI.Data;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

public class FuncionarioService : IFuncionarioService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;

    public FuncionarioService(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public string GenerateSenhaHash(string senha)
    {
        string hashSenha = BCrypt.Net.BCrypt.HashPassword(senha, Key.Salt);
        return hashSenha;
    }

    public async Task<ReadFuncionarioDto> CreateFuncionario(CreateFuncionarioDto funcionarioDto)
    {
        var funcionario = _mapper.Map<Funcionario>(funcionarioDto);
        funcionario.Senha = GenerateSenhaHash(funcionarioDto.Senha);

        await _context.Funcionarios.AddAsync(funcionario);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadFuncionarioDto>(funcionario);
    }

    public async Task<IEnumerable<ReadFuncionarioDto>> GetAllFuncionarios()
    {
        var funcionarios = await _context.Funcionarios.ToListAsync();
        return _mapper.Map<List<ReadFuncionarioDto>>(funcionarios);
    }

    public async Task<ReadFuncionarioDto> GetFuncionarioDto(int id)
    {
        var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Id == id);
        if (funcionario == null){return null;}

        return _mapper.Map<ReadFuncionarioDto>(funcionario);
    }

    public async Task UpdateFuncionario(int id, UpdateFuncionarioDto funcionarioDto)
    {
        var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Id == id);
        if (funcionario == null)
        {
            throw new KeyNotFoundException("Funcionário não encontrado");
        }

        _mapper.Map(funcionarioDto, funcionario);
        funcionario.Senha = GenerateSenhaHash(funcionarioDto.Senha);

        await _context.SaveChangesAsync();
    }
}