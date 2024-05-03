using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateAndUpdateUsuarioDto, Usuario>();
        CreateMap<Usuario, ReadUsuarioDto>();
        CreateMap<CreateAndUpdateUsuarioDto, Usuario>();
    }
}
