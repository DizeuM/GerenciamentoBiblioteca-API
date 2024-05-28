using AutoMapper;
using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Profiles;

public class ExemplarProfile : Profile
{
    public ExemplarProfile()
    {
        CreateMap<Exemplar, ReadExemplarDto>();
    }
}
