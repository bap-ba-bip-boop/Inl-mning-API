using AutoMapper;
using Inlämning_API.DTO;
using Inlämning_API.Model;

namespace Inlämning_API.Infrastructure.Profiles;

public class AdsProfile : Profile
{
    public AdsProfile()
    {
        //CreateMap<AdItemsDTO, Advertisement>();
        CreateMap<Advertisement, AdItemsDTO>();
        CreateMap<Advertisement, AdDTO>();
        CreateMap<CreateAdDTO, Advertisement>();
        CreateMap<EditAdDTO, Advertisement>();
    }
}
