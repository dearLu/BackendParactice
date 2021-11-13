using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{    
    /// <summary>
    /// 2.1.1 - Создать новую сущность-агрегатор (LibraryCard): человек, взявший для прочтения книгу + дата и время
    /// получения книги (DateTimeOffset) 
    /// </summary>
    public class LibraryCard
    {
        /// <summary>
        /// 2.2.1 - Добавьте валидации в ваши сущности: все обязательные поля должны быть NotNull. 
        /// </summary>
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан читатель")]
        public HumanDTO Reader { get; set; }

        [Required(ErrorMessage = "Не указана книга")]
        public BookDTO Book { get; set; }

        /// <summary>
        /// 2.1.5 - Использовать формат даты и времени yyyy-MM-ddTHH:mm:ss.fffzzz (2021-01-01T16:01:12.257+04:00)
        /// </summary>
        public string DateTimeGetBook 
        {
            
            get 
            { 
                return DateTimeGetBook;
            }

            set
            {
                DateTimeGetBook = DateTimeOffset.Now.ToString("o");
                                       
            }        
        }
    }
}
