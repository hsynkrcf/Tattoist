using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.Entities
{
    [Table("Brokers")]
    public class Broker
    {
        [Key]
        public Guid BrokerId { get; set; }
        public string BrokerTitle { get; set; }
        public string BrokerPhone { get; set; }
        public int BrokerPercent { get; set; }
        public string RelatedPerson { get; set; }
        public bool PaymentType { get; set; }
        public string Iban { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        [NotMapped]
        public string NameWithPercent { get { return string.Format("{0} - {1}", RelatedPerson, BrokerPercent); } }
    }
}
