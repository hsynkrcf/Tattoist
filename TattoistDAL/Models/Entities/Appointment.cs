using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.Entities
{
    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        public Guid AppointmentId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Phone { get; set; }
        public string WorkType { get; set; }
        public int DepositeAmount { get; set; }
        public string DepositeType { get; set; }
        public Guid PaymentId { get; set; }
        public string TattooArtist { get; set; }
        public string PiercingArtist { get; set; }
        public string Broker { get; set; }
        public string Designer { get; set; }
        public bool IsClosed { get; set; }
        public decimal TotalPrice { get; set; }
        public string PriceType { get; set; }
    }
}
