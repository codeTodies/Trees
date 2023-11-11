using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnNhom1.Models
{
    public class Area
    {
        public Area()
        {
            this.Trees = new HashSet<Tree>();
            this.ImgArea = "~/image/BatTay.jpg";
        }
        [Key]
        public int IDArea { get; set; }
        [Required]
        public string ImgArea { get; set; }
        [Required]
        public string NameArea { get; set; }
        public int IDRe { get; set; }

        [ForeignKey("IDRe")]
        public virtual Region Region { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
        [NotMapped]
        public List<Area> ListArea { get; set; }
        public virtual ICollection<Tree> Trees { get; set; }
    }
}