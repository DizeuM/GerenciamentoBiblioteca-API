using AutoMapper;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Profiles;

public class MultaProfile : Profile
{
    public MultaProfile() 
    {
        CreateMap<Multa, ReadMultaDto>();

    }
}
