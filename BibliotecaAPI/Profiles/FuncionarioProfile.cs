using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
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

