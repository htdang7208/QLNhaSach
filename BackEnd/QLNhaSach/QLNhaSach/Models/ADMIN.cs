using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaSach.Models
{
    [Table("Admin")]
    public class ADMIN
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public string imageName { get; set; }
        public string url { get; set; }
        public bool isRemove { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
    }
}