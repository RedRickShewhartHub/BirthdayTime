using BirthdayTime.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BirthdayTime.Data;

public class AppDbContext : DbContext
{
    public DbSet<BirthdayEntry> Birthdays { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.BirthdayEntryConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
