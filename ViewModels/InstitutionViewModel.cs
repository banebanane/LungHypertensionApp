using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.ViewModels
{
    public class InstitutionViewModel
    {
        [Required]
        public string Id { get; set; } // institution name
        [Required]
        public string InstitutionAddress { get; set; }
        [Required]
        public string InstitutionHolder { get; set; }
        public string SearchName { get; set; }
        public long TimeStamp { get; set; }
    }
}
