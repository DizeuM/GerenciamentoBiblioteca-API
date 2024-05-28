using AutoMapper;
using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data.Models;
using BibliotecaAPI.Enums;

namespace BibliotecaAPI.Profiles;

public class ExemplarProfile : Profile
{
    public ExemplarProfile()
    {
        CreateMap<Exemplar, ReadExemplarDto>();
    }
}
