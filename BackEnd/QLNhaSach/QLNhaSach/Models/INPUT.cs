using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QLNhaSach.Models
{
    [Table("Input")]
    public class INPUT
    {
        [Key]
        public int id { get; set; }
        public bool isRemove { get; set; }

        public ICollection<INPUTDETAIL> INPUTDETAILS { get; set; }
    }
}
