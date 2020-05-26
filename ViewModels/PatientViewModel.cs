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
        public int SearchIdInstitution { get; set; }
        public IEnumerable<string> EnumWHO { get; set; }
        public IEnumerable<string> EnumEKG { get; set; }
        public IEnumerable<string> EnumRisk { get; set; }

    }
}
