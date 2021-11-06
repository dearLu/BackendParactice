using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    /// <summary>
    /// 1.2.2 - Класс книги
    /// </summary>
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public HumanDTO Author { get; set; }
        public string Genre { get; set; }
    }
}
