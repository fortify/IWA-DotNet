using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroFocus.InsecureWebApp.Models;

namespace MicroFocus.InsecureWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MicroFocus.InsecureWebApp.Models.Product> Product { get; set; }

        public DbSet<Prescription> Prescription { get; set; }

        public DbSet<Order> Order { get; set; }
    }
}
