using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.DTO
{
    public class CashReportVM
    {
        public decimal ExpenseSum { get; set; }
        public decimal IncomeSum { get; set; }
        public decimal Sum { get; set; }
        public decimal Cash { get; set; }
        public decimal CreditCard { get; set; }
        public decimal Remittance { get; set; }
        public string ReportRange { get; set; }
    }
}
