using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.Data.Entities
{
    public class Institution
    {
        public string Id { get; set; } // Institution name
        public string InstitutionAddress { get; set; }
        public string InstitutionHolder { get; set; }
        public long TimeStamp { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}
