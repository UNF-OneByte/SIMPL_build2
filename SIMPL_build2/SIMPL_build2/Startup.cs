using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMPL.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIMPL.Models;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace SIMPL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
/**
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
**/
            services.AddDbContext<project_trackerContext>(options =>
          options.UseSqlServer(
             Configuration.GetConnectionString("DefaultConnection")));
            services
                .AddIdentityCore<AspNetUsers>()
                .AddRoles<AspNetRoles>()
                .AddEntityFrameworkStores<project_trackerContext>();


            services.AddScoped<IUserClaimsPrincipalFactory<AspNetUsers>, AppClaimsPrincipalFactory>();

    //        services.AddDbContext<project_trackerContext>(options =>
    //            options.UseSqlServer(
    //                Configuration.GetConnectionString("DefaultConnection")));
    //        services.AddDefaultIdentity<IdentityUser>()
    //            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
           
        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<AspNetRoles>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AspNetUsers>>();

            IdentityResult roleResult;
            //Adding Admin Role 
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database 
                roleResult = await RoleManager.CreateAsync(new AspNetRoles("Admin"));
            }
            //Assign Admin role to the main User here we have given our newly registered  
            //login id for Admin management 
            AspNetUsers user = await UserManager.FindByEmailAsync("syedshanumcain@gmail.com");
            var User = new AspNetUsers();
            await UserManager.AddToRoleAsync(user, "Admin");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();

            CreateUserRoles(services).Wait();
        }

    }
}
