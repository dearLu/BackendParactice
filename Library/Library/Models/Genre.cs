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
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string GenreName { get; set; }

        [NotMapped]
        public virtual ICollection<Book> Books { get; set; }
    }
}
