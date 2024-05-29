using AutoMapper;
using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;
using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDto, Usuario>();
        CreateMap<Usuario, ReadUsuarioDto>()
            .ForMember(dest => dest.IdsEmprestimos, opt => opt.MapFrom(src => src.Emprestimos.Where(e => e.Status != EmprestimoStatus.Devolvido).Select(m => m.Id)))
            .ForMember(dest => dest.IdsMultas, opt => opt.MapFrom(src => src.Multas.Where(m => m.Status == MultaStatus.Pendente).Select(m => m.Id)));
        CreateMap<UpdateUsuarioDto, Usuario>();
    }
}
