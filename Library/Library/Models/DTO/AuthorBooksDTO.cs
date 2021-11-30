using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.DTO
{
    public class AuthorBooksDto
    {
        public AuthorDto Author { get; set; }
        public List<BookDto> Books { get; set; }

    }
}
