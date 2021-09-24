using ShoppingCart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure
{
    public class ShoppingCartContext : IdentityDbContext<AppUser>
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {
        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}