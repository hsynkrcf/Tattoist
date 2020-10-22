using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.Entities
{
    [Table("AppointmentPayments")]
    public class Payment
    {
        [Key]
        public Guid LineId { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid CustomerId { get; set; }
        public int PaymentAmount { get; set; }
        public int PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
