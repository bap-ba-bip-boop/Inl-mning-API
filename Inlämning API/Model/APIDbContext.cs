using Microsoft.EntityFrameworkCore;

namespace Inlämning_API.Model;

public class APIDbContext : DbContext
{
    public DbSet<Advertisement>? advertisements { get; set; }
    public APIDbContext(DbContextOptions<APIDbContext> options) : base(options) {}
}
