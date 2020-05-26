using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LungHypertensionApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LungHypertensionApp.Data
{
    public class LungHypertensionRepository : ILungHypertensionRepository
    {
        private readonly LungHypertensionContext context;
        private readonly ILogger<LungHypertensionRepository> logger;

        public LungHypertensionRepository(LungHypertensionContext context, ILogger<LungHypertensionRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void AddEntity(object model)
        {
            context.Add(model);
        }


        public IEnumerable<Institution> GetAllInstitutions()
        {
            //logger.LogInformation("Test da li loguje");
            try
            {
                return context.Institutions.ToList();
                // context.Products.Include(o => o.Items).ThenInclude(i => i.Product).ToList(); omogucava da json vrati i listu foreig kljuceva
            }
            catch (Exception)
            {
                logger.LogError("Could not obtain institutions from database.");
            }
            return new List<Institution>();
        }

        public Institution GetInstitutionById(string id)
        {
            try
            {
                return context.Institutions.Where(i => i.Id == id).FirstOrDefault();
            }
            catch(Exception)
            {
                logger.LogError("Could not obtain institution from database.");
            }
            return new Institution();
        }

        //public IEnumerable<Product> GetProductsByCategory(string category)
        //{
        //    return context.Products.Where(p => p.Category == category).ToList();
        //}

        public bool SaveAll()
        {
            return context.SaveChanges() > 0;
        }

        public void SaveInstitution(Institution institution)
        {
            try
            {
                context.Institutions.Add(institution);
            }
            catch (Exception)
            {
                logger.LogError("Could not save institution from database.");
            }            
        }

        public void UpdateInstitution(Institution institution)
        {
            try
            {
                context.Institutions.Update(institution);
            }
            catch (Exception)
            {
                logger.LogError("Could not update institution from database.");
            }
        }

        public void DeleteInstitution(Institution institution)
        {
            try
            {
                // obrisati uvezanu tabelu korisnika
                context.RemoveRange(context.Users.Where(i => i.InstitutionName.Id == institution.Id));
                context.Institutions.Remove(institution);
            }
            catch (Exception)
            {
                logger.LogError("Could not delete institution from database.");
            }
        }

        public StoreUser GetUserByEmail(string email)
        {
            try
            {
                // obrisati uvezanu tabelu korisnika
                return context.Users.Where(i => i.Email == email).FirstOrDefault();
            }
            catch (Exception)
            {
                logger.LogError("Could not get user from database.");
            }

            return null;
        }

        public bool UpdateUserRole(StoreUser user, string role)
        {
            try
            {  
                IdentityRole idNewRole = context.Roles.Where(i => i.Name == role).FirstOrDefault();

                IdentityUserRole<string> userRoleCurrent = context.UserRoles.Where(u => u.UserId == user.Id).FirstOrDefault();
                if (idNewRole != null && userRoleCurrent != null)
                {
                    userRoleCurrent.RoleId = idNewRole.Id;
                    context.UserRoles.Update(userRoleCurrent);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                logger.LogError("Could not create user role to database.");
                return false;
            }
        }

        public void DeleteUser(StoreUser user)
        {
            try
            {
                // Uvezan rekord user role bi trebalo da se obrise rekurzivno
                context.Users.Remove(user);
            }
            catch (Exception)
            {
                logger.LogError("Could not delete user from database.");
            }
        }
    }
}
