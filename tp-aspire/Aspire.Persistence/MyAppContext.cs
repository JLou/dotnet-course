namespace Aspire.Persistence;

using Microsoft.EntityFrameworkCore;

public class MyAppContext : DbContext
{
    public MyAppContext(DbContextOptions<MyAppContext> options)
        : base(options) { }

    public DbSet<Item> Items => Set<Item>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Item>().ToTable("Items");
        modelBuilder.Entity<Item>().HasKey(i => i.Id);
        modelBuilder.Entity<Item>().Property(i => i.Value).IsRequired();
    }
}
