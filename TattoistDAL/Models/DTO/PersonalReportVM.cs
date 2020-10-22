using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.DTO
{
    public class PersonalReportVM
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public decimal Amount { get; set; }
        public int Percent { get; set; }
        public decimal AmountPercent { get; set; }
        public string ReportRange { get; set; }
    }
}
