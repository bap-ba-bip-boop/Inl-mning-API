
public interface ICreateUniqeService
{
    public enum CreateUniqueStatus
    {
        Ok,
        AlreadyExists,
        Error
    }
    public CreateUniqueStatus CreateIfNotExists<S>(DbSet<S>set, Func<S,bool> compareFunc, S ItemToAdd) where S : class;
}