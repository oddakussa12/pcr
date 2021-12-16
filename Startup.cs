using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CORE_MVC_EMP_MGMT.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Session;

namespace ASP.NET_CORE_MVC_EMP_MGMT
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        //dependency injection
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
          
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));
            //to use Identity framework
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            
            
            //for using session in our applicatioin
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set session for 1 day after login.
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSession();
            //SQLEmployeeRepository provides implementation for IEmployeeRepository
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            services.AddScoped<IProjectRepository, SQLProjectRepository>();
            services.AddScoped<IRisk, SQLRiskRepository>();
            services.AddScoped<IActionoRepository, SQLActionoRepository>();
            services.AddScoped<IIssueRepository, SQLIssueRepository>();
            services.AddScoped<IScheduleRepository, SQLScheduleRepository>();

            services.AddAuthentication();

            
            
        }//end ConfigureServices method

        //request processing pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //put it before UseMvc middleware, this middle ware is used for Identity framework
            app.UseAuthentication();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=index}/{id?}");
            });

        }//end configure method
    }//end Startup class
}
