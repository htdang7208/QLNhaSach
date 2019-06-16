using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models
{
    [Table("SaleDetail")]
    public class SALEDETAIL
    {
        [Key]
        public int stt { get; set; }
        public int saleId { get; set; }
        public int bookId { get; set; }
        public int amount { get; set; }
        public double totalPrice { get; set; }

        [ForeignKey("saleId")]
        public SALE SALE { get; set; }
        [ForeignKey("bookId")]
        public BOOK BOOK { get; set; }
    }
}
