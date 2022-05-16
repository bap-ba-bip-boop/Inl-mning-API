using Inlämning_API.Model;

namespace Inlämning_API.Services;

public interface IAdAPIService
{
    public enum AdAPIStatus
    {
        AdExists,
        AdDoesNotExist
    }
    public (AdAPIStatus, Advertisement) VerifyAccountID(int Id, APIDbContext _context);
}