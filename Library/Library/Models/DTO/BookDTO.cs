using Library.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    /// <summary>
    /// 1.2.2 - Класс книги
    /// </summary>
    public class BookDto
    {
        /// <summary>
        /// 2.2.1 - Добавьте валидации в ваши сущности: все обязательные поля должны быть NotNull. 
        /// </summary>
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Не указан автор")]
        public AuthorDto Author { get; set; }

        [Required(ErrorMessage = "Не указан жанр")]
      
        public  List<GenreDto> Genres { get; set; }
       
        public  List<HumanDto> Persons { get; set; }

    }
}
