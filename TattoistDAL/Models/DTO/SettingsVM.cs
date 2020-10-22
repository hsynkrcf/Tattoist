using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.DTO
{
    public class SettingsVM
    {
        public int CreditCardTax { get; set; }
        public int KDV { get; set; }
        public string BirthdayScheduling { get; set; }
        public string AppointmentScheduling { get; set; }
        public string AppointmentText { get; set; }
        public string BirthdayText { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
    }
}
