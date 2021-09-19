using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure
{
    public class ShoppingCartContext: DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
            :base(options)
        {
           
        }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
