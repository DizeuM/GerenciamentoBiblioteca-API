using AutoMapper;
using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;
using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Profiles;

public class LivroProfile : Profile
{
    public LivroProfile() 
    {
        CreateMap<CreateLivroDto, Livro>();
        CreateMap<Livro, ReadLivroDto>()
            .ForMember(dest => dest.QntExemplarDisponivel, opt => opt.MapFrom(src => src.Exemplares.Where(e => e.Status == ExemplarStatus.Disponivel).Count()));
        CreateMap<UpdateLivroDto, Livro>();

    }
}
