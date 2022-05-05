using Inlämning_API.Model;
using static Inlämning_API.Services.IAccountAPIService;

namespace Inlämning_API.Services;

public class AccountAPIService : IAccountAPIService
{
    public (AccountStatus, Advertisement) VerifyAccountID(int Id, APIDbContext _context)
    {
        var ad = _context.advertisements!.FirstOrDefault(ad => ad.Id == Id);
        return ( (ad == null)? AccountStatus.AccountDoesNotExist : AccountStatus.AccountExists, ad )!;
    }
}