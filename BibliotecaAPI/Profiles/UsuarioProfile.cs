using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Enums;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDto, Usuario>();
        CreateMap<Usuario, ReadUsuarioDto>()
            .ForMember(dest => dest.Emprestimos, opt => opt.MapFrom(src => src.Emprestimos.Where(e => e.Status == EmprestimoStatus.EmAndamento || e.Status == EmprestimoStatus.Atrasado).Select(e => e.Id))); ;
        CreateMap<UpdateUsuarioDto, Usuario>();
    }
}
