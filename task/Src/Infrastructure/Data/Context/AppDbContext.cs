using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using task.Src.Domain.Entities.Identity;
using task.Src.Domain.Entities.Organization;

namespace task.Src.Infrastructure.Data.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<UserUnitRole> UserUnitRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<OrganizationUnit>()
            .HasMany(o => o.Children)
            .WithOne(o => o.ParantUnit)
            .HasForeignKey(o => o.ParentUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserUnitRole>()
            .HasOne(ur => ur.User)
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserUnitRole>()
            .HasOne(ur => ur.organizationUnit)
            .WithMany()
            .HasForeignKey(ur => ur.OrganizationUnitId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
