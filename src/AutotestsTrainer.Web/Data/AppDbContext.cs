using AutotestsTrainer.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AutotestsTrainer.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasIndex(x => x.UserName).IsUnique();
            entity.Property(x => x.UserName).HasMaxLength(64).IsRequired();
            entity.Property(x => x.DisplayName).HasMaxLength(128).IsRequired();
            entity.Property(x => x.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<WorkItem>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(1000);
            entity.Property(x => x.Status).HasMaxLength(32).IsRequired();
            entity.HasOne(x => x.OwnerUser)
                .WithMany(x => x.WorkItems)
                .HasForeignKey(x => x.OwnerUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
