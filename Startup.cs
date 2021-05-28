using Hangfire;
using Hangfire.MySql;
using LeftRightNet.Data;
using LeftRightNet.Hangfire;
using LeftRightNet.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace LeftRightNet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string AppDBConnectionString = Configuration.GetConnectionString("DefaultConnection");
            string AppHangfireConnectionString = Configuration.GetConnectionString("DefaultConnection_Hangfire");

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseMySql(AppDBConnectionString, new MySqlServerVersion(new Version(8, 0, 25))));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            services.AddHangfire(configuration => configuration.UseStorage(new MySqlStorage(
                    AppHangfireConnectionString,
                    new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                        QueuePollInterval = TimeSpan.FromSeconds(5),
                        JobExpirationCheckInterval = TimeSpan.FromMinutes(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(1),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 50000,
                        TransactionTimeout = TimeSpan.FromMinutes(30),
                        TablesPrefix = "Hangfire_LeftRight_"
                    })));

            services.AddHttpClient("GetHeadLines");

            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
           

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangFireAuthorizationFilter() }
            });
            HangFireJobScheduler.ScheduleRecurringJobs();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                RoleManager<IdentityRole> RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<ApplicationUser> UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                //var JobsRepo = scope.ServiceProvider.GetRequiredService<IJobPostingRepository>();
                //await JobsRepo.BuildCache();
                ApplicationDbContext content = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                IdentityResult roleResult;

                //Adding Admin Role
                bool roleCheck = await RoleManager.RoleExistsAsync("Admin");
                if (!roleCheck)
                {
                    //create the roles and seed them to the database
                    roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
                }

                //Assign Admin role to the main User here we have given our newly registered 
                //login id for Admin management
                // Also Assigning them Claims to perform CUD operations
                ApplicationUser user = await UserManager.FindByEmailAsync("avaneesab5@gmail.com");
                if (user != null)
                {
                    IList<string> currentUserRoles = await UserManager.GetRolesAsync(user);
                    if (!currentUserRoles.Contains("Admin"))
                    {
                        await UserManager.AddToRoleAsync(user, "Admin");
                    }

                    
                }
            }
        }
    }
}
