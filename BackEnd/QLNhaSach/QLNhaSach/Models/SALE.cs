using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models
{
    [Table("Sale")]
    public class SALE
    {
        [Key]
        public int id { get; set; }
        public int customerId { get; set; }
        public DateTime dateCreated { get; set; }
        public bool isRemove { get; set; }

        [ForeignKey("customerId")]
        public CUSTOMER CUSTOMER { get; set; }
    }
}
