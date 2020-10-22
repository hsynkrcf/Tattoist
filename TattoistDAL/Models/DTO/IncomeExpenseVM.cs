using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.DTO
{
    public class IncomeExpenseVM
    {
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public string ReportRange { get; set; }
    }
}
