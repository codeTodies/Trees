using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnNhom1.Models
{
    public class Region
    {
        public Region()
        {
            this.Areas = new HashSet<Area>();
            this.ImgRe = "~/image/BatTay.jpg";
        }
        [Key]
        public int IDRe { get; set; }
        [Required]
        public string ImgRe { get; set; }
        [Required]
        public string NameRe { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
        [NotMapped]
        public List<Region> ListRe { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
    }
}