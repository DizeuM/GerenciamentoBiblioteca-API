using AutoMapper;
using BibliotecaAPI.Data.Dtos.Response;
using BibliotecaAPI.Data.Models;

namespace BibliotecaAPI.Profiles;

public class RenovacaoProfile : Profile
{
    public RenovacaoProfile()
    {
        CreateMap<Renovacao, ReadRenovacaoDto>();

    }
}
