using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.DTO
{
    public class DepositVM
    {
        public int Amount { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
    }
}
