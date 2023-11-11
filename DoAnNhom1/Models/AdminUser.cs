using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnNhom1.Models
{
    public class AdminUser
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string NameUser { get; set; }
        [Required]
        public string RoleUser { get; set; }
        [Required]
        public string PasswordUser { get; set; }
    }
}