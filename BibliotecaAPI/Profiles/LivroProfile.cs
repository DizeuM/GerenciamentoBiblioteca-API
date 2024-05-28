using AutoMapper;
using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Profiles;

public class LivroProfile : Profile
{
    public LivroProfile() 
    {
        CreateMap<CreateLivroDto, Livro>();
        CreateMap<Livro, ReadLivroDto>();
        CreateMap<UpdateLivroDto, Livro>();

    }
}
