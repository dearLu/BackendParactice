using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class GenreDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string GenreName { get; set; }

        public  List<BookDto> Books { get; set; }
    }
}
