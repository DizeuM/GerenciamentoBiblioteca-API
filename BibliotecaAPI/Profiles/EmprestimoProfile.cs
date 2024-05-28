using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Profiles;

public class EmprestimoProfile : Profile
{
    public EmprestimoProfile() 
    {
        CreateMap<CreateEmprestimoDto, Emprestimo>();
        CreateMap<Emprestimo, ReadEmprestimoDto>();
    }
}
