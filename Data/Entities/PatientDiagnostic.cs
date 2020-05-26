using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.Data.Entities
{
    public class PatientDiagnostic
    {
        public int Id { get; set; }
        public string EKG { get; set; }

        public string Risk { get; set; }
    }
}
