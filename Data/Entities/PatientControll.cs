using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.Data.Entities
{
    public class PatientControll
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public PatientBaseData PatientId { get; set; }
        [Required]
        public DateTime ControllDate { get; set; }  
        public string WeekHearth { get; set; } 
    }
}
