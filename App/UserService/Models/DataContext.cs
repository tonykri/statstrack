using Microsoft.EntityFrameworkCore;

namespace UserService.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opt ) : base(opt)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<ProfessionalLife> ProfessionalLife { get; set; }
    public DbSet<PersonalLife> PersonalLife { get; set; }
    public DbSet<EmailCode> EmailCodes { get; set; }
    public DbSet<Hobby> Hobbies { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Expenses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Hobbies)
            .WithOne(h => h.User)
            .HasForeignKey(h => h.UserId);
        
        modelBuilder
            .Entity<User>()
            .HasOne(u => u.UserPersonalLife)
            .WithOne(p => p.User)
            .HasForeignKey<PersonalLife>(p => p.UserId);

        modelBuilder
            .Entity<User>()
            .HasOne(u => u.UserProfessionalLife)
            .WithOne(p => p.User)
            .HasForeignKey<ProfessionalLife>(p => p.UserId);

        modelBuilder
            .Entity<User>()
            .HasOne(u => u.UserEmailCode)
            .WithOne(c => c.User)
            .HasForeignKey<EmailCode>(c => c.UserId);

        modelBuilder
            .Entity<Expense>()
            .HasKey(e => new {e.UserId, e.UserExpense});
        
        modelBuilder
            .Entity<Hobby>()
            .HasKey(e => new {e.UserId, e.UserHobby});
    }
}