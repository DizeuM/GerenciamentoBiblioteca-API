using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;

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
