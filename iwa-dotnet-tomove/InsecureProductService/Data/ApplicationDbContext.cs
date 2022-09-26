using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MicroFocus.InsecureProductService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MicroFocus.InsecureProductService.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        public DbSet<MicroFocus.InsecureProductService.Models.Product> Product { get; set; }
    }
}
