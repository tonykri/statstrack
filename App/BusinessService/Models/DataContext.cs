using Microsoft.EntityFrameworkCore;

namespace BusinessService.Models;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> opt ) : base(opt)
    { 
    }

    public DbSet<Business> Businesses { get; set; }
    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Business>()
            .HasMany(b => b.Photos)
            .WithOne(p => p.Business)
            .HasForeignKey(p => p.BusinessId);
        
        modelBuilder
            .Entity<Photo>()
            .HasKey(p => new {p.BusinessId, p.PhotoId});
    }
}