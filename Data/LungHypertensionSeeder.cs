using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using LungHypertensionApp.Data.Entities;

namespace LungHypertensionApp.Data
{
    public class LungHypertensionSeeder
    {
        private readonly LungHypertensionContext context;
        private readonly UserManager<StoreUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public LungHypertensionSeeder(LungHypertensionContext context, UserManager<StoreUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            context.Database.EnsureCreated(); // da budemo sigurni da se kreirala baza pre upisa podataka

            // Adding roles: Admin, Manager, User

            bool adminExists = await roleManager.RoleExistsAsync("Admin");
            if (!adminExists)
            {
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                await roleManager.CreateAsync(role);
            }

            // creating Manager role     
            bool managerExists = await roleManager.RoleExistsAsync("Manager");
            if (!managerExists)
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                await roleManager.CreateAsync(role);
            }

            // creating Creating User role     
            bool userExists = await roleManager.RoleExistsAsync("User");
            if (!userExists)
            {
                var role = new IdentityRole();
                role.Name = "User";
                await roleManager.CreateAsync(role);
            }

            //Add one default Institution

            Institution institution = new Institution()
            {
                Id = "Sremska Kamenica",
                InstitutionAddress = "Vladike Platona 12",
                InstitutionHolder = "Zoran Potic",
                TimeStamp = DateTime.UtcNow.Ticks
            };

            //Here we create a Admin super user who will maintain the website   

            StoreUser user = await userManager.FindByEmailAsync("daki0505@gmail.com");

            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Vladimir",
                    LastName = "Potic",
                    Email = "daki0505@gmail.com",
                    UserName = "daki0505@gmail.com",
                    Titule = "programer",
                    InstitutionName = institution
                };

                var result = await userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in Seeder");
                }
                else // add regular user to admin
                {
                    result = await userManager.AddToRoleAsync(user, "Admin");

                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not Admin permission to User");
                    }
                }
            }

            context.SaveChanges(); // provereno ovo radi u internoj EF transakciji!
        }
    }
}
