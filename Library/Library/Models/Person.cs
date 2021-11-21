using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    /// <summary>
    /// 2.2	Подготовить в приложении сущности согласно созданной ранее базе данных 
    /// </summary>
    public class Person
    {
        [Key]
        public int Id { get; set; }

        public DateTime BirthDate{get;set;}

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        [NotMapped]
        public virtual ICollection<Book> Books { get; set; }
    }
}
