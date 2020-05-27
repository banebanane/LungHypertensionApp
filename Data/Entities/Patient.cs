using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.Data.Entities
{
    public class Patient
    {
        #region Base data
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long TimeStamp { get; set; }
        public ICollection<PatientControll> Controls { get; set; }
        public Institution Institution { get; set; }
        #endregion

        #region Contact
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        #endregion

        #region Diagnostic
        public string EKG { get; set; }

        public string Risk { get; set; }
        #endregion

        #region
        public string WHO { get; set; }
        public double NtProBnp { get; set; }
        #endregion

        #region
        public double Hgb { get; set; }
        public double Hct { get; set; }
        #endregion

        #region Theraphy
        #endregion
    }
}
