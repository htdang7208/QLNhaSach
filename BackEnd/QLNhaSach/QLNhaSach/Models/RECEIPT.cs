using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models
{
    [Table("Receipt")]
    public class RECEIPT
    {
        [Key]
        public int id { get; set; }
        public int customerId { get; set; }
        public DateTime datePaid { get; set; }
        public double payment { get; set; }
        public bool isRemove { get; set; }

        [ForeignKey("customerId")]
        public CUSTOMER CUSTOMER { get; set; }
    }
}
