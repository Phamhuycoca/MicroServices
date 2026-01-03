using AuthService.Domain.Entities;
using AuthService.Infrastructure.AppContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API;

public static class ConfigureServices
{
    public static IServiceCollection AddApicontrollerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<ApplicationDbContext>(builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentity<nguoi_dung, IdentityRole<Guid>>(options =>
        {
            // Cấu hình password
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            // User
            options.User.RequireUniqueEmail = true;

            // Lockout
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();


        services.ConfigureApplicationCookie(opt =>
        {
            opt.LoginPath = "/Auth/Auth_Login";
            opt.AccessDeniedPath = "/Auth/AccessDenied";
        });

        return services;
    }
}