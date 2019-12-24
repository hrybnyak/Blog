using BLL.Interfaces;
using BLL.Services;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using DAL.Entities;

namespace BLL.Infrastructure
{
    public static class InjectionResolver
    {
        public static void Inject(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
            services.AddIdentity<User, IdentityRole>(o => {
                o.User.RequireUniqueEmail = true;
                o.Password.RequiredLength = 6;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddSingleton<IJwtFactory, JwtFactory>();
        }
    }
}
