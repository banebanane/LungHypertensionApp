using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.Data.Entities
{
    public class PatientContactData
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
