using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.DTO
{
    public class AppointmentList
    {
        public string AppointmentId { get; set; }
        public string CustomerName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Phone { get; set; }
        public string WorkType { get; set; }
        public int DepositeAmount { get; set; }
        public string DepositeType { get; set; }
        public string TattoistName { get; set; }
        public string PiercingistName { get; set; }
        public string BrokerName { get; set; }
        public string DesignerName { get; set; }
        public bool IsClosed { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
