using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.Data.Entities
{
    public class PatientFunctional
    {
        public int Id { get; set; }
        public string WHO { get; set; }
        public double NtProBnp { get; set; }
    }
}
