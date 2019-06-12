using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models
{
    [Table("Book")]
    public class BOOK
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string kind { get; set; }
        public string author { get; set; }
        public int stock { get; set; }
        public bool state { get; set; }
        public bool isRemove { get; set; }

        public string imageName { get; set; }
        public string url { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }


        public ICollection<INPUTDETAIL> INPUTDETAILS { get; set; }
    }
}
