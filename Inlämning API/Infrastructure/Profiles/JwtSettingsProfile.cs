using AutoMapper;
using Inlämning_API.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Inlämning_API.Infrastructure.Profiles;

public class JwtSettingsProfile : Profile
{
    public JwtSettingsProfile()
    {
        CreateMap<JwtSettings, TokenValidationParameters>()
            .ForMember(
                src => src.IssuerSigningKey,
                opt => opt.MapFrom(src => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(src.Key!)))
            );
    }
}
