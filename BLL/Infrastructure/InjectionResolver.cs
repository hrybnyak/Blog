using BLL.Interfaces;
using BLL.Services;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using DAL.Entities;
using DAL.Interfaces;
using DAL.UnitOfWork;

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
                o.Password.RequireDigit = false;
                o.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IArticleService, ArticleService>();
        }
    }
}
