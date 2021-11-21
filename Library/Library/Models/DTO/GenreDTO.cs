using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class GenreDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string GenreName { get; set; }

        public  List<BookDTO> Books { get; set; }
    }
}
