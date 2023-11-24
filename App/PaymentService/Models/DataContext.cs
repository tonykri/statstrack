using Microsoft.EntityFrameworkCore;

namespace PaymentService.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opt ) : base(opt)
    {
        
    }

    public DbSet<Business> Businesses { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Business>()
            .HasMany(b => b.Payments)
            .WithOne(p => p.Business)
            .HasForeignKey(p => p.BusinessId);
    }
}