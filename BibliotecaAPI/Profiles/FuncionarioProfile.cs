using AutoMapper;
using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Profiles;

public class FuncionarioProfile : Profile
{
    public FuncionarioProfile()
    {
        CreateMap<CreateFuncionarioDto, Funcionario>();
        CreateMap<Funcionario, ReadFuncionarioDto>();
        CreateMap<UpdateFuncionarioDto, Funcionario>();
    }
}

