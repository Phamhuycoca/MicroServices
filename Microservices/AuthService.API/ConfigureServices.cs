using AuthService.Domain.Entities;
using AuthService.Infrastructure.AppContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthService.API;

public static class ConfigureServices
{
    public static IServiceCollection AddApicontrollerServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<ApplicationDbContext>(builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        //Cấu hình cors
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                 policy => policy

                                .WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials());
        });
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
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();


        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "AUTH_COOKIE";
            options.LoginPath = "/Auth/Auth_Login";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(7); // nhớ 7 ngày
            options.SlidingExpiration = true;
            if (env.IsDevelopment())
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            }
            else
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            }

        });


        return services;
    }
}