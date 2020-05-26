using System.Collections.Generic;
using LungHypertensionApp.Data.Entities;

namespace LungHypertensionApp.Data
{
    public interface ILungHypertensionRepository
    {
        IEnumerable<Institution> GetAllInstitutions();
    //    IEnumerable<Product> GetProductsByCategory(string category);

        bool SaveAll();
        void AddEntity(object model);
        Institution GetInstitutionById(string id); // get by name
        void SaveInstitution(Institution institution);
        void UpdateInstitution(Institution institution);
        void DeleteInstitution(Institution institution);

        StoreUser GetUserByEmail(string email);
        bool UpdateUserRole(StoreUser user, string role);
        void DeleteUser(StoreUser user);
    }
}