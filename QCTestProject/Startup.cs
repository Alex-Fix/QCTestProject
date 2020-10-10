using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QCTestProject.Models;
using Microsoft.EntityFrameworkCore;
using QCTestProject.Services;

namespace QCTestProject
{
    public class Startup
    {
        public IConfiguration Configuratin { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuratin = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string dbConnection = Configuratin.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(dbConnection));
            services.AddTransient<CacheService>();
            services.AddMemoryCache();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
