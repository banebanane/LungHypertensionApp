using LungHypertensionApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long TimeStamp { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public double NtProBNP { get; set; }
        public string WHO { get; set; }
        public double Hgb { get; set; }
        public double Hct { get; set; }
        public string EKG { get; set; }
        public string Risk { get; set; }
        public int SearchIdPatient { get; set; }
        public IEnumerable<string> AllInstitutions { get; set; }
        public string InstitutionName { get; set; }
        public IEnumerable<string> EnumWHO { get; set; }
        public IEnumerable<string> EnumEKG { get; set; }
        public IEnumerable<string> EnumRisk { get; set; }

        public void ResetProperties()
        {
            Id = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            Telephone = string.Empty;
            Mobile = string.Empty;
            Email = string.Empty;
            NtProBNP = 0;
            Hgb = 0;
            Hct = 0;
            SearchIdPatient = 0;
        }

    }
}
