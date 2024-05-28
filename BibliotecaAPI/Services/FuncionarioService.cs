using AutoMapper;
using BibliotecaAPI;
using BibliotecaAPI.Data;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data.Models;
using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Enums;
using BibliotecaAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

public class FuncionarioService : IFuncionarioService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public FuncionarioService(BibliotecaContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public string GenerateSenhaHash(string senha)
    {
        string hashSenha = BCrypt.Net.BCrypt.HashPassword(senha, _configuration["PasswordHash:Salt"]);
        return hashSenha;
    }

    public async Task<Funcionario> GetFuncionarioByIdOrThrowError(int id)
    {
        var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Id == id);
        if (funcionario == null)
        {
            throw new NotFoundException("Funcionário não encontrado.");
        }

        return funcionario;
    }

    public async Task<ReadFuncionarioDto> CreateFuncionario(CreateFuncionarioDto funcionarioDto)
    {
        var funcionario = _mapper.Map<Funcionario>(funcionarioDto);
        funcionario.Senha = GenerateSenhaHash(funcionarioDto.Senha);
        funcionario.Status = FuncionarioStatus.Ativo;

        await _context.Funcionarios.AddAsync(funcionario);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadFuncionarioDto>(funcionario);
    }

    public async Task<IEnumerable<ReadFuncionarioDto>> SearchFuncionarioByAttributes(SearchFuncionarioDto searchFuncionarioDto)
    {
        var query = _context.Funcionarios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchFuncionarioDto.Nome))
        {
            query = query.Where(f => f.Nome.ToLower().Contains(searchFuncionarioDto.Nome.ToLower().Trim()));
        }

        if (!string.IsNullOrWhiteSpace(searchFuncionarioDto.Cpf))
        {
            query = query.Where(f => f.Cpf == searchFuncionarioDto.Cpf);
        }

        if (!string.IsNullOrWhiteSpace(searchFuncionarioDto.Email))
        {
            query = query.Where(f => f.Email.ToLower().Contains(searchFuncionarioDto.Email.ToLower().Trim()));
        }

        if (searchFuncionarioDto.Status != null)
        {
            query = query.Where(f => f.Status == searchFuncionarioDto.Status);
        }

        var funcionarios = await query.ToListAsync();

        return _mapper.Map<List<ReadFuncionarioDto>>(funcionarios);
    }

    public async Task<IEnumerable<ReadFuncionarioDto>> GetAllFuncionarios()
    {
        var funcionarios = await _context.Funcionarios.ToListAsync();

        return _mapper.Map<List<ReadFuncionarioDto>>(funcionarios);
    }

    public async Task<ReadFuncionarioDto> GetFuncionarioById(int id)
    {
        var funcionario = await GetFuncionarioByIdOrThrowError(id);

        return _mapper.Map<ReadFuncionarioDto>(funcionario);
    }

    public async Task UpdateFuncionario(int id, UpdateFuncionarioDto funcionarioDto)
    {
        var funcionario = await GetFuncionarioByIdOrThrowError(id);

        _mapper.Map(funcionarioDto, funcionario);

        funcionario.Senha = GenerateSenhaHash(funcionarioDto.Senha);

        await _context.SaveChangesAsync();
    }
}