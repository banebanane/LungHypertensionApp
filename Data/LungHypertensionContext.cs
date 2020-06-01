using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LungHypertensionApp.Data.Entities;

namespace LungHypertensionApp.Data
{
    public class LungHypertensionContext : IdentityDbContext<StoreUser>
    {
        public LungHypertensionContext(DbContextOptions<LungHypertensionContext> options) : base(options)
        {

        }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientControll> PatientControlls { get; set; }
    }
}
