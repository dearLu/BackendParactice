using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.DTO
{
    public class PersonBooks
    {
        public HumanDTO Human { get; set; }
        public List<BookDTO> Books { get; set; }
    }
}
