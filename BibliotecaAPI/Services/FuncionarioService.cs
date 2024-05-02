using AutoMapper;
using BibliotecaAPI;
using BibliotecaAPI.Data;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;

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

    public ReadFuncionarioDto CreateFuncionario(CreateFuncionarioDto funcionarioDto)
    {
        var funcionario = _mapper.Map<Funcionario>(funcionarioDto);
        funcionario.Senha = GenerateSenhaHash(funcionarioDto.Senha);

        _context.Funcionarios.Add(funcionario);
        _context.SaveChanges();

        return _mapper.Map<ReadFuncionarioDto>(funcionario);
    }

    public IEnumerable<ReadFuncionarioDto> GetAllFuncionarios()
    {
        var funcionarios = _context.Funcionarios.ToList();
        return _mapper.Map<List<ReadFuncionarioDto>>(funcionarios);
    }

    public ReadFuncionarioDto GetFuncionarioDto(int id)
    {
        var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == id);
        if (funcionario == null){return null;}

        return _mapper.Map<ReadFuncionarioDto>(funcionario);
    }

    public void UpdateFuncionario(int id, UpdateFuncionarioDto funcionarioDto)
    {
        var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == id);
        if (funcionario == null)
        {
            throw new KeyNotFoundException("Funcionário não encontrado");
        }

        _mapper.Map(funcionarioDto, funcionario);
        funcionario.Senha = GenerateSenhaHash(funcionarioDto.Senha);

        _context.SaveChanges();
    }

    public void DeleteFuncionario(int id)
    {
        var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == id);
        if (funcionario == null)
        {
            throw new KeyNotFoundException("Funcionário não encontrado");
        }

        _context.Funcionarios.Remove(funcionario);
        _context.SaveChanges();
    }
}