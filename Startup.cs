
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LungHypertensionApp.Data;
using LungHypertensionApp.Services;
using Newtonsoft.Json;
using LungHypertensionApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LungHypertensionApp
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
           {
               cfg.User.RequireUniqueEmail = true;
           }).AddEntityFrameworkStores<LungHypertensionContext>();

            services.AddTransient<IMailService, NullMailService>();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddDbContext<LungHypertensionContext>(cfg =>
                {
                    cfg.UseSqlServer(config.GetConnectionString("LungHypertensionConnectionStringProd"));
                });
            }
            else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                services.AddDbContext<LungHypertensionContext>(cfg =>
                {
                    cfg.UseSqlServer(config.GetConnectionString("LungHypertensionConnectionString"));
                });
            }


            services.AddTransient<LungHypertensionSeeder>();
            services.AddControllersWithViews(); // for mvc
            services.AddScoped<ILungHypertensionRepository, LungHypertensionRepository>(); // ovde se moze menjati sa nekim mochrepository
            services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore); // da li nam treba?
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error"); // Ovo mi nije proradilo
            }
            app.UseStaticFiles();
            //app.UseNodeModules();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(cfg =>
            {
                cfg.MapControllerRoute("Fallback",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", action = "Index" });
            }); // for usage of mvc
        }
    }
}
