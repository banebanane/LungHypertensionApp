using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.Data.Entities
{
    public class PatientLab
    {
        public int Id { get; set; }
        public double Hgb { get; set; }
        public double Hct { get; set; }
    }
}
