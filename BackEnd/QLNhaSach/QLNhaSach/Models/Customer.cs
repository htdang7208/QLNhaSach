using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaSach.Models
{
    [Table("Customer")]
    public class CUSTOMER
    {
        [Key]
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public double dept { get; set; }

        public bool isRemove { get; set; }
        public string imageName { get; set; }
        public string url { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }

        public ICollection<RECEIPT> RECEIPTS { get; set; }
    }
}