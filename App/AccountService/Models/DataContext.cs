using Microsoft.EntityFrameworkCore;

namespace AccountService.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opt ) : base(opt)
    {
        
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<EmailCode> EmailCodes { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Account>()
            .HasMany(a => a.EmailCodes)
            .WithOne(e => e.Account)
            .HasForeignKey(e => e.AccountId);

        modelBuilder
            .Entity<Account>()
            .HasMany(a => a.RefreshTokens)
            .WithOne(e => e.Account)
            .HasForeignKey(e => e.AccountId);
    }
}