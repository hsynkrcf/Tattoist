using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;

namespace TattoistDAL.Models.Entities
{
    [Table("Users")]
    public class User 
    {
        [Key]
        public Guid Id { get; set; }
        [DisplayName("Kullanıcı Tipi")]
        public int UserType { get; set; }        
        [DisplayName("Adı Soyadı")]
        public string FullName { get; set; }
        public string Email { get; set; }
        [DisplayName("Kullanıcı Adı")]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}