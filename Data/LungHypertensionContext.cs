using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LungHypertensionApp.Data.Entities;

namespace LungHypertensionApp.Data
{
    public class LungHypertensionContext : IdentityDbContext<StoreUser>
    {
        public LungHypertensionContext(DbContextOptions<LungHypertensionContext> options) : base(options)
        {

        }
        public DbSet<Institution> Institutions { get; set; }
    //    public DbSet<Order> Orders { get; set; }
    }
}
