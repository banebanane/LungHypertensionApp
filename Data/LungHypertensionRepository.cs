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
                logger.LogError("Could not save institution to database.");
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

        public Patient GetPatientById(int id)
        {
            try
            {
                return context.Patients.Include(ins => ins.Institution).Where(i => i.Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                logger.LogError($"Could not get patient with {id} from database.");
            }

            return null;
        }

        public IEnumerable<PatientControll> GetPatientControlsByPatientId(int id)
        {
            try
            {
                return context.PatientControlls.Where(i => i.Patient.Id == id);
            }
            catch (Exception)
            {
                logger.LogError($"Could not get patient controlls for patient with {id} from database.");
            }

            return null;
        }

        public IEnumerable<Patient> GetAllPatientsByInstitution(string institutionName)
        {
            try
            {
                return context.Patients.Where(i => i.Institution.Id == institutionName);
            }
            catch (Exception)
            {
                logger.LogError($"Could not get patients from {institutionName} from database.");
            }

            return null;
        }

        public void UpdatePatient(Patient patient)
        {
            try
            {
                context.Patients.Update(patient);
            }
            catch (Exception)
            {
                logger.LogError("Could not update patient from database.");
            }
        }

        public bool UpdatePatientControlByPatientId(int id)
        {
            throw new NotImplementedException();
        }

        public int GetPatientMaxId()
        {
            try
            {
                if (context.Patients.Any())
                {
                    return context.Patients.Max(i => i.Id);
                }

                return 0;         
            }
            catch (Exception)
            {
                logger.LogError($"Could not get max patient's id from database.");
            }

            return -1;
        }

        public void SavePatient(Patient patient)
        {
            try
            {
                context.Patients.Add(patient);
            }
            catch (Exception)
            {
                logger.LogError("Could not save Patient to database.");
            }
        }

        public void DeletePatient(Patient patient)
        {
            try
            {
                context.RemoveRange(context.PatientControlls.Where(p => p.Patient.Id == patient.Id));
                context.Patients.Remove(patient);
            }
            catch (Exception)
            {
                logger.LogError("Could not delete patient from database.");
            }
        }

        public void SavePatientControll(PatientControll patientControll)
        {
            try
            {
                context.PatientControlls.Add(patientControll);
            }
            catch (Exception)
            {
                logger.LogError("Could not save Patient controll to database.");
            }
        }

        public IEnumerable<PatientControll> GetAllControllsForPatient(int patientId)
        {
            try
            {
                return context.PatientControlls.Where(cont => cont.Patient.Id == patientId);
            }
            catch (Exception)
            {
                logger.LogError($"Could not obtain controlls by patient with ID: {patientId} from database.");
            }
            return new List<PatientControll>();
        }

        public Dictionary<DateTime, string> GetAllControlsParamForPatientIdAndParam(int patientId, string param)
        {
            try // GetType().GetProperty(param).GetValue(i, null).ToString() pomocu refleksije bi mozda moglo da se izvrsi izvlacenje obicnih propertyja
            {              
                return context.PatientControlls.Include(p => p.Patient).Where(cont => cont.Patient.Id == patientId).ToDictionary(i => i.ControllDate, i => i.Patient.Id.ToString());
            }
            catch (Exception)
            {
                logger.LogError($"Could not obtain controlls by patient with ID: {patientId} from database.");
            }
            return new Dictionary<DateTime, string>();
        }
    }
}
