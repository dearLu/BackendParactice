using Library.Models;
using Library.Models.DTO;
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

        public static List<HumanDTO> AllHuman = new List<HumanDTO>()
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
                Id = 3,
                Name = "Александр",
                Surname = "Александров",
                Patronymic = "Александрович",
                Birthday = new DateTime(1990, 1, 3)

            }
        };
        public static List<AuthorDTO> AllAuthor = new List<AuthorDTO>()
        {
            new AuthorDTO
            {
                Id = 2,
                FirstName = "Александр",
                LastName = "Пушкин",
                MiddleName = "Сергеевич",
                Books = AllBook,

            }
        };

        public static List<GenreDTO> AllGenres = new List<GenreDTO>()
        {
            new GenreDTO
            {
                Id = 1,
                GenreName = "роман",
                Books= AllBook.Where(e=>e.Genres.Any(e=>e.GenreName == "роман")).ToList()

            },
            new GenreDTO
            {
                Id = 2,
                GenreName = "повесть",
                Books= AllBook.Where(e=>e.Genres.Any(e=>e.GenreName == "повесть")).ToList()

            },

        };
        public static List<BookDTO> AllBook = new List<BookDTO>()
        {
            new BookDTO
            {
                Id = 1,
                Title = "Капитанская дочка",
                Author = AllAuthor.Where(e=> e.LastName == "Пушкин").FirstOrDefault(),
                Genres = AllGenres.Where(e=>e.Id == 1).ToList(),

            },
            new BookDTO
            {
                Id = 2,
                Title = "Пиковая дама",
                Author = AllAuthor.Where(e=> e.LastName == "Пушкин").FirstOrDefault(),
                Genres = AllGenres.Where(e=>e.Id == 2).ToList(),

            },
            new BookDTO
            {
                Id = 3,
                Title = "Дубровский",
                Author = AllAuthor.Where(e=> e.LastName == "Пушкин").FirstOrDefault(),
                Genres = AllGenres.Where(e => e.Id == 1).ToList(),

            },

        };

        /// <summary>
        /// 2.1.3 - пустой статичный список, отвечающий за хранение этих сущностей(LibraryCard)
        /// </summary>
        public static List<LibraryCard> Cards = new List<LibraryCard>() { };
    }
}
