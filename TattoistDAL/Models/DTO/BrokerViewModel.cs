using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.DTO
{
    public class BrokerViewModel
    {
        public string brokerid { get; set; }
        public string Title { get; set; }
        public string Percent { get; set; }
        public string RelatedPerson { get; set; }
        public string Phone { get; set; }
        public bool PaymentType { get; set; }
        public string Iban { get; set; }
        public string Account { get; set; }
        public string Branch { get; set; }
        public string BankName { get; set; }
    }
}
