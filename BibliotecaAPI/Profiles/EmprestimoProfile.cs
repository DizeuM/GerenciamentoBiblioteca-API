using AutoMapper;
using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Profiles;

public class EmprestimoProfile : Profile
{
    public EmprestimoProfile() 
    {
        CreateMap<CreateEmprestimoDto, Emprestimo>();
        CreateMap<Emprestimo, ReadEmprestimoDto>();
    }
}
