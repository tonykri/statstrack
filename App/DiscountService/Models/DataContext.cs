using Microsoft.EntityFrameworkCore;

namespace DiscountService.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opt ) : base(opt)
    {
        
    }

    public DbSet<Business> Businesses { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Business>()
            .HasMany(b => b.Coupons)
            .WithOne(c => c.Business)
            .HasForeignKey(c => c.BusinessId);
    }
}