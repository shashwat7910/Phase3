using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LappyShop.Models
{
    public class AppDbContext : IdentityDbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> products { get;set; }
        public DbSet<Order> orders { get; set; }


    }
}
