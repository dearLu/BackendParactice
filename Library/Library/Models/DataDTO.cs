using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{   
    /// <summary>
    /// 1.2.3 - Статичные списки людей и книг
    /// </summary>
    public class DataDTO
    {
        public static List<HumanDTO> allHuman = new List<HumanDTO>()
        {
            new HumanDTO
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Patronymic = "Иванович",
                Birthday = new DateTime(2000, 2, 1)

            },
            new HumanDTO
            {
                Id = 2,
                Name = "Александр",
                Surname = "Пушкин",
                Patronymic = "Сергеевич",
                Birthday = new DateTime(1799, 6, 6)

            },
            new HumanDTO
            {
                Id = 3,
                Name = "Александр",
                Surname = "Александров",
                Patronymic = "Александрович",
                Birthday = new DateTime(1990, 1, 3)

            }
        };

        public static List<BookDTO> allBook = new List<BookDTO>()
        {
            new BookDTO
            {
                Id = 1,
                Title = "Капитанская дочка",
                Author = allHuman.Where(e=> e.Surname == "Пушкин").FirstOrDefault(),
                Genre = "роман" 

            },
            new BookDTO
            {
                Id = 2,
                Title = "Пиковая дама",
                Author = allHuman.Where(e=> e.Surname == "Пушкин").FirstOrDefault(),
                Genre = "повесть"

            },
            new BookDTO
            {
                Id = 3,
                Title = "Дубровский",
                Author = allHuman.Where(e=> e.Surname == "Пушкин").FirstOrDefault(),
                Genre = "роман"

            },

        };

        /// <summary>
        /// 2.1.3 - пустой статичный список, отвечающий за хранение этих сущностей(LibraryCard)
        /// </summary>
        public static List<LibraryCard> cards = new List<LibraryCard>() { };
    }
}
