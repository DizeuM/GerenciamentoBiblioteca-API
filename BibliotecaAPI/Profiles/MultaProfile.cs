using AutoMapper;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Profiles;

public class MultaProfile : Profile
{
    public MultaProfile() 
    {
        CreateMap<Multa, ReadMultaDto>();

    }
}
