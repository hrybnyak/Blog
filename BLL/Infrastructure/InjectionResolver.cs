using BLL.Interfaces;
using BLL.Services;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Infrastructure
{
    public static class InjectionResolver
    {
        public static void Inject(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));


            services.AddSingleton<IJwtFactory, JwtFactory>();
        }
    }
}
