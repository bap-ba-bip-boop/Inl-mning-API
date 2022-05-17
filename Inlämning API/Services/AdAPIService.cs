using Inlämning_API.Model;
using static Inlämning_API.Services.IAdAPIService;

namespace Inlämning_API.Services;

public class AdAPIService : IAdAPIService
{
    public (AdAPIStatus, Advertisement) VerifyAccountID(int Id, APIDbContext _context)
    {
        var ad = _context.advertisements!.FirstOrDefault(ad => ad.Id == Id);
        return ( (ad == default)? AdAPIStatus.AdDoesNotExist : AdAPIStatus.AdExists, ad )!;
    }
}