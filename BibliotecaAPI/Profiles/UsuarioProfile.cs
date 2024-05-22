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
            .ForMember(dest => dest.IdsEmprestimos, opt => opt.MapFrom(src => src.Emprestimos.Where(e => e.Status == EmprestimoStatus.EmAndamento || e.Status == EmprestimoStatus.Atrasado).Select(e => e.Id)))
            .ForMember(dest => dest.IdsMultas, opt => opt.MapFrom(src => src.Multas.Where(m => m.Status == MultaStatus.Pendente).Select(m => m.Id)));
        CreateMap<UpdateUsuarioDto, Usuario>();
    }
}
