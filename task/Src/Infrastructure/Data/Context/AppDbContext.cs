using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using task.Src.Domain.Common;
using task.Src.Domain.Entities.Identity;
using task.Src.Domain.Entities.Organization;
using task.Src.Domain.Entities.Tasks;

namespace task.Src.Infrastructure.Data.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<UserUnitRole> UserUnitRoles { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<OrganizationUnit>()
            .HasMany(o => o.Children)
            .WithOne(o => o.ParantUnit)
            .HasForeignKey(o => o.ParentUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrganizationUnit>().HasQueryFilter(o => o.DeleteAt == null);

        builder.Entity<UserUnitRole>()
            .HasOne(ur => ur.User)
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserUnitRole>()
            .HasOne(ur => ur.OrganizationUnit)
            .WithMany()
            .HasForeignKey(ur => ur.OrganizationUnitId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;
            else if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}