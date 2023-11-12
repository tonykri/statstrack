using Microsoft.EntityFrameworkCore;

namespace ReviewService.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opt ) : base(opt)
    {
        
    }

    public DbSet<Business> Businesses { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<VerifiedOrder> VerifiedOrders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Business>()
            .HasMany(b => b.Responses)
            .WithOne(r => r.Business)
            .HasForeignKey(r => r.BusinessId);
        
        modelBuilder
            .Entity<Business>()
            .HasMany(b => b.VerifiedOrders)
            .WithOne(v => v.Business)
            .HasForeignKey(v => v.BusinessId);
        
        modelBuilder
            .Entity<Business>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Business)
            .HasForeignKey(r => r.BusinessId);
        
        modelBuilder
            .Entity<VerifiedOrder>()
            .HasKey(v => new {v.UserId, v.BusinessId});

        modelBuilder
            .Entity<Review>()
            .HasOne(r => r.Response)
            .WithOne(r => r.Review)
            .HasForeignKey<Response>(r => r.ReviewId)
            .IsRequired(false);
    }
}