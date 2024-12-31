using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Data;
using Microsoft.AspNetCore.Identity;
using ShoppingCart.Models;
namespace ShoppingCart
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ShoppingCartContext>(options => {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("ShoppingCartContext")
                    ?? throw new InvalidOperationException("Connection string 'ShoppingCartContext' not found."),
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(180); // Increase timeout for long-running operations
                    }
                );
            });


            // define user roles
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ShoppingCartContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Apply pending migrations
                var context = services.GetRequiredService<ShoppingCartContext>();
                context.Database.Migrate();

                // seed data
                await SeedData.Initialize(services); // initialize the data in SeedData model
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}