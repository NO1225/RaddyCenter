using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RaddyCenter.Data.DataAccess;
using System;
using System.Threading.Tasks;

namespace RaddyCenter
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            EnsureRolesExists(serviceProvider).GetAwaiter().GetResult();
        }

        private async Task EnsureRolesExists(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            // Ensure database exist
            await context.Database.MigrateAsync();

            // Ensure admin role exists
            var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
            if (adminRoleExists == false)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            }

            // Ensure moderator role exists
            var moderatorRoleExists = await roleManager.RoleExistsAsync("Moderator");
            if (moderatorRoleExists == false)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Moderator" });
            }

            // Ensure admin user exist
            var adminUser = await userManager.FindByNameAsync("appAdmin");
            if(adminUser == null)
            {
                adminUser = new IdentityUser()
                {
                    Email = "appAdmin@RaddyCenter.com",
                    UserName = "appAdmin"
                };
                await userManager.CreateAsync(adminUser, "@ppAdm1n");
            }

            //var token = await userManager.GenerateEmailConfirmationTokenAsync(adminUser);
            //await userManager.ConfirmEmailAsync(adminUser, token);


            // Ensure that the admin has the admin role
            var userRoles =  await userManager.GetRolesAsync(adminUser);
            if(userRoles.Contains("Admin") == false)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

        }
    }
}
