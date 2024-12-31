using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public class ShoppingCartContext : IdentityDbContext
    {
        public ShoppingCartContext (DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {
        }
        public DbSet<ShoppingCart.Models.Item> Item { get; set; } = default!;

        public DbSet<ShoppingCart.Models.Cart> Cart { get; set; } = default!;
    }
}
