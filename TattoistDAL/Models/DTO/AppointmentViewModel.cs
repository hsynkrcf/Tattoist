using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TattoistDAL.Models.Entities;

namespace TattoistDAL.Models.DTO
{
    public class AppointmentViewModel
    {
        public SelectList Customers { get; set; }
        public SelectList Tattoist { get; set; }
        public SelectList Piercingist { get; set; }
        public SelectList BrokerList { get; set; }
        public SelectList DesignerList { get; set; }
    }
}
