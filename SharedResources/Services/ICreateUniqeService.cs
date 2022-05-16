using Microsoft.EntityFrameworkCore;

namespace SharedResources.Services;

public interface ICreateUniqeService
{
    public enum CreateUniqueStatus
    {
        Ok,
        AlreadyExists
    }
    public CreateUniqueStatus CreateIfNotExists<S>(DbContext _dbContext, DbSet<S>set, Func<S,bool> compareFunc, S ItemToAdd) where S : class;
}