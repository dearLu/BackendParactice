using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.DTO
{
    public class AuthorBooksGenresDTO
    {
        public AuthorDTO Author { get; set; }
        public BookDTO Book{ get; set; }
        public List<GenreDTO> Genre { get; set; }
    }
}
