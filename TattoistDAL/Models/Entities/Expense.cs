using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TattoistDAL.Models.Entities
{
    [Table("Expenses")]
    public class Expense
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public string Time { get; set; }
    }
}
