using System.Collections.Generic;

namespace QLNhaSach.Models
{
    public class BookReport
    {
        public string name { get; set; }
        public int oldStock { get; set; }
        public int nowStock { get; set; }
        public int additionalStock { get; set; }
    }
    public class ListBookReport
    {
        public List<BookReport> listBookReport { get; set; }
    }
}