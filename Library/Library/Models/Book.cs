using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Book
    {
        public Book()
        {
            this.Genres = new HashSet<Genre>();
        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }

        [NotMapped]
        public virtual ICollection<Genre> Genres { get; set; }
        [NotMapped]
        public virtual ICollection<Person> Persons { get; set; }
    }
}
