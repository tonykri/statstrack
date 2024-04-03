namespace StatisticsService.Models;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opt) : base(opt)
    {

    }

    public DbSet<Business> Businesses { get; set; }
    public DbSet<UserStats> UserStats { get; set; }
    public DbSet<PersonalStats> PersonalStats { get; set; }
    public DbSet<IndustryStats> IndustryStats { get; set; }
    public DbSet<IncomeStats> IncomeStats { get; set; }
    public DbSet<HobbyStats> HobbyStats { get; set; }
    public DbSet<ExpenseStats> ExpenseStats { get; set; }
    public DbSet<EducationStats> EducationStats { get; set; }
    public DbSet<WorkingHoursStats> WorkingHoursStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}