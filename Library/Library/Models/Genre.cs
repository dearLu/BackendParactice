using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Genre
    {
        public Genre()
        {
            this.Books = new HashSet<Book>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string GenreName { get; set; }

        [NotMapped]
        public virtual ICollection<Book> Books { get; set; }
    }
}
