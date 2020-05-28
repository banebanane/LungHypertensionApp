using LungHypertensionApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.ViewModels
{
    public class PatientControllViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public DateTime ControllDate { get; set; }
        public string WeekHearth { get; set; }
        public long TimeStamp { get; set; }
        public string ParamName { get; set; }
        public Dictionary<DateTime, string> DateTimeToValue { get; set; }
        public IEnumerable<PatientControll> PatientControlls { get; set; }
    }
}
