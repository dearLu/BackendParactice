using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.DTO
{
    public class AuthorBooksGenresDto
    {
        public AuthorDto Author { get; set; }
        public BookDto Book{ get; set; }
        public List<GenreDto> Genre { get; set; }
    }
}
