using AuthService.Domain.BaseEntity;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.AppContext;

public class ApplicationDbContext : IdentityDbContext<nguoi_dung, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<nguoi_dung>(b =>
        {
            b.ToTable("nguoi_dung");
        });

        builder.Entity<IdentityRole<Guid>>(b =>
        {
            b.ToTable("vai_tro");
        });

        builder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.ToTable("nguoi_dung_vai_tro");
        });

        builder.Entity<IdentityUserClaim<Guid>>(b =>
        {
            b.ToTable("nguoi_dung_claim");
        });

        builder.Entity<IdentityUserLogin<Guid>>(b =>
        {
            b.ToTable("nguoi_dung_login");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(b =>
        {
            b.ToTable("vai_tro_claim");
        });

        builder.Entity<IdentityUserToken<Guid>>(b =>
        {
            b.ToTable("nguoi_dung_token");
        });
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var userName = "anonymous";
        foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.ngay_tao = DateTime.Now;
                    entry.Entity.ngay_chinh_sua = DateTime.Now;
                    entry.Entity.nguoi_chinh_sua = userName;
                    entry.Entity.nguoi_tao = userName;
                    break;
                case EntityState.Modified:
                    entry.Entity.ngay_tao = DateTime.Now;
                    entry.Entity.ngay_chinh_sua = DateTime.Now;
                    entry.Entity.nguoi_chinh_sua = userName;
                    entry.Entity.nguoi_tao = userName;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}