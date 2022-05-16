using Inlämning_API.Model;

namespace Inlämning_API.Services;

public interface IAccountAPIService
{
    public enum AccountStatus
    {
        AccountExists,
        AccountDoesNotExist
    }
    public (AccountStatus, Advertisement) VerifyAccountID(int Id, APIDbContext _context);
}