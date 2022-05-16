using Microsoft.EntityFrameworkCore;
using SharedResources.Services.ICreateUniqeService;

namespace SharedResources.Services;

public class CreateUniqeService : ICreateUniqeService
{
    public CreateUniqueStatus CreateIfNotExists<S>(DbSet<S>set, Func<S,bool> compareFunc, S ItemToAdd) where S : class
    {
        if ( set.Any(compareFunc) )
            return CreateUniqueStatus.AlreadyExists;
        set.Add(ItemToAdd);
        _dbContext.SaveChanges();
        return CreateUniqueStatus.Ok;
    }
}