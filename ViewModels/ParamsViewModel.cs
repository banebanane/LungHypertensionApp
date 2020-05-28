using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.ViewModels
{
    public class ParamsViewModel
    {
        public int PatientID { get; set; }
        public string ParamName { get; set; }
        public Dictionary<DateTime, string> DateTimeToValue { get; set; }
    }
}
