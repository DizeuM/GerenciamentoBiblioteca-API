using AutoMapper;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Profiles;

public class RenovacaoProfile : Profile
{
    public RenovacaoProfile()
    {
        CreateMap<Renovacao, ReadRenovacaoDto>();

    }
}
