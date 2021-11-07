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
        public HumanDTO reader { get; set; }

        [Required(ErrorMessage = "Не указана книга")]
        public BookDTO book { get; set; }

        public DateTimeOffset dateTimeGetBook 
        {
            
            get { return dateTimeGetBook; }

            set {
                    dateTimeGetBook = DateTimeOffset.ParseExact(
                                        DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz",               CultureInfo.InvariantCulture),
                                        "yyyy-MM-ddTHH:mm:ss.fffzzz",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None);
                }        
        }
    }
}
