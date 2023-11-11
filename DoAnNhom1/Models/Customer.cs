using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnNhom1.Models
{
    public class Customer
    {
        [Key]
        public int IDCus { get; set; }
        [Required]
        public string NameCus { get; set; }
        [Required]
        public string PhoneCus { get; set; }
        [Required]
        public string EmailCus { get; set; }
    }
}