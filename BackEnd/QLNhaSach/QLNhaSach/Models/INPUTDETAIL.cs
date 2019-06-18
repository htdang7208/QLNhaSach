using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models
{
    [Table("InputDetail")]
    public class INPUTDETAIL
    {
        [Key]
        public int stt { get; set; }
        public int bookId { get; set; }
        public int inputId { get; set; }
        public int amount { get; set; }
        
        [ForeignKey("bookId")]
        public BOOK BOOK { get; set; }

        [ForeignKey("inputId")]
        public INPUT INPUT { get; set; }
    }
}
