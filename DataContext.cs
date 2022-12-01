using Microsoft.EntityFrameworkCore;

namespace EFCoreDemo;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<Blog> Blogs { get; set; }
}
