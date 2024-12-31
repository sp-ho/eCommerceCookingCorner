using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;

namespace ShoppingCart.Models
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ShoppingCartContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ShoppingCartContext>>()))
            {
                // Check if database has been seeded for Items
                if (!context.Item.Any())
                {
                    // Seed data for items
                    context.Item.AddRange(
                        new Item
                        {
                            Name = "Cutlery Set, 16-piece",
                            Price = 20,
                            Quantity = 100,
                            ImageUrl = "https://www.ikea.com/ca/en/images/products/mopsig-16-piece-cutlery-set__0759952_pe750550_s5.jpg?f=xl",
                            Category = "Utensils",
                            IsClearance = false,
                            IsBestSeller = false
                        },
                        new Item
                        {
                            Name = "Pot with Lid, 2.8 L",
                            Price = 30,
                            Quantity = 50,
                            ImageUrl = "https://www.ikea.com/ca/en/images/products/ikea-365-pot-with-lid-stainless-steel__1006173_pe825758_s5.jpg?f=s",
                            Category = "Pots",
                            IsClearance = false,
                            IsBestSeller = false
                        },
                        new Item
                        {
                            Name = "Frying Pan, 24 cm (9\")",
                            Price = 15,
                            Quantity = 10,
                            ImageUrl = "https://www.ikea.com/ca/en/images/products/kavalkad-frying-pan-black__0241981_pe381624_s5.jpg?f=s",
                            Category = "Pans",
                            IsClearance = false,
                            IsBestSeller = false
                        },
                        new Item
                        {
                            Name = "Wok, 33 cm (13\")",
                            Price = 20,
                            Quantity = 45,
                            ImageUrl = "https://www.ikea.com/ca/en/images/products/tolerant-wok-black__0710361_pe727488_s5.jpg?f=s",
                            Category = "Woks",
                            IsClearance = false,
                            IsBestSeller = false
                        },
                        new Item
                        {
                            Name = "Bowl",
                            Price = 5,
                            Quantity = 2,
                            ImageUrl = "https://tse1.mm.bing.net/th?id=OIP.lz8IvP7VrOwr0jJ3eDEVJwHaHa&pid=Api&P=0&h=180",
                            Category = "Dinnerware",
                            IsClearance = false,
                            IsBestSeller = true
                        }
                    );
                    await context.SaveChangesAsync();
                }
            }

            // Seed Roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Admin User
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string adminEmail = "admin@cookscorner.com";
            string adminPassword = "Abcd@1234";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
