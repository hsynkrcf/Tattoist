using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.DTO
{
    public class AppointmentJsonVM
    {
        public string Customer { get; set; }
        public string AppointmentDate { get; set; }
        public string Phone { get; set; }
        public string WorkType { get; set; }
        public int DepositeAmount { get; set; }
        public string DepositeType { get; set; }
        public string PriceType { get; set; }
        public string TattooArtist { get; set; }
        public string PiercingArtist { get; set; }
        public string Broker { get; set; }
        public string Designer { get; set; }
        public bool IsClosed { get; set; }
        public decimal TotalPrice { get; set; }
        public List<string> PaymentList { get; set; }
        public List<string> PaymentDateList { get; set; }
        public List<string> PaymentTypeList { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string HeaderId { get; set; }
    }
}
