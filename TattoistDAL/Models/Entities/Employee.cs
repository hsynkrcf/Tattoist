using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TattoistDAL.Models.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }
        public int EmployeeType { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeePercent { get; set; }
        public string Phone { get; set; }
        [NotMapped]
        public string NameWithPercent { get { return string.Format("{0} - {1}", EmployeeName, EmployeePercent); } }
    }
}
