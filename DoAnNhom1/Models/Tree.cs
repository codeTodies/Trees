using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnNhom1.Models
{
    public class Tree
    {
        public Tree()
        {
            this.ImageTree = "~/image/BatTay.jpg";
        }
        [Key]
        public int TreeID { get; set; }
        [Required]
        public string NameTree { get; set; }
        [Required]
        public string DescriptionTree { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ImageTree { get; set; }
        public int? IDArea { get; set; }
        [ForeignKey("IDArea")]
        public virtual Area Area { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
    }
}