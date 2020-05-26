using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.Data.Entities
{
    public class StoreUser : IdentityUser
    {
        public Institution InstitutionName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Titule { get; set; }
    }
}
