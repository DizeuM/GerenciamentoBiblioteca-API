using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
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
