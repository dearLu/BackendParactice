using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests
{
    public class DataTest
    {
        public static List<Genre> genres = new List<Genre>()
        {
            new Genre
            {
                Id = 1,
                GenreName = "роман",
                Books = books.Where(e=>e.Genres.Any(e=>e.GenreName == "роман")).ToList()
            },
            new Genre
            {
                Id = 2,
                GenreName = "научная фантастика",
                Books = books.Where(e=>e.Genres.Any(e=>e.GenreName == "научная фантастика")).ToList()
            },
            new Genre
            {
                Id = 3,
                GenreName = "проза",
                Books = books.Where(e=>e.Genres.Any(e=>e.GenreName == "проза")).ToList()
            },
            new Genre
            {
                Id = 4,
                GenreName = "хобби",
                Books = books.Where(e=>e.Genres.Any(e=>e.GenreName == "хобби")).ToList()
            }
        };

        public static List<Person> persons = new List<Person>()
        {   new Person
            {
                Id = 1,
                BirthDate = new DateTime(1960, 1, 1),
                FirstName = "Александр",
                LastName = "Александров",
                MiddleName = "Александрович",
                Books= books.Where(e=>e.Genres.Any(e=>e.GenreName == "хобби")).ToList()
            },

            new Person
            {
                Id = 2,
                BirthDate = new DateTime(2000, 05, 07),
                FirstName = "Петр",
                LastName = "Иванов",
                MiddleName = "Александрович",
                Books= books.Where(e=>e.Genres.Any(e=>e.GenreName == "роман")).ToList()
            },

            new Person
            {
                Id = 3,
                BirthDate = new DateTime(1999, 05, 03),
                FirstName = "Светлана",
                LastName = "Александрова",
                MiddleName = "Сергеевна",
                Books= books.Where(e=>e.Genres.Any(e=>e.GenreName == "проза")).ToList()
            }
        };

        public static List<Author> authors = new List<Author>()
        {
            new Author
            {
                Id = 1,
                FirstName = "Александр",
                LastName = "Пушкин",
                MiddleName = "Сергеевич",
                //Books = books.Where(e=>e.Author.LastName == "Пушкин").ToList()
            },
            new Author
            {
                Id = 2,
                FirstName = "Пелам",
                LastName = "Вудхаус",
                MiddleName = "Гренвилл",
                //Books = books.Where(e=>e.Author.LastName == "Вудхаус").ToList()
            },
            new Author
            {
                Id = 3,
                FirstName = "Джеймс",
                LastName = "Гарни",
                MiddleName = "",
                //Books = books.Where(e=>e.Author.LastName == "Гарни").ToList()
            },
            new Author
            {
                Id = 4,
                FirstName = "Кобо",
                LastName = "Абэ",
                MiddleName = "",
               // Books = books.Where(e=>e.Author.LastName == "Абэ").ToList()
            },
            new Author
            {
                Id = 5,
                FirstName = "Исаак",
                LastName = "Азимов",
                MiddleName = "Юдович",
                //Books = books.Where(e=>e.Author.LastName == "Азимов").ToList()
            },
        };

        public static List<Book> books = new List<Book>()
        {
            new Book
            {
                Id = 1,
                Name = "Цвет и свет",
                Author = authors[3],
                Genres = genres.Where(e=>e.GenreName == "хобби").ToList(),
                Persons = persons.Take(2).ToList()
            },
            new Book
            {
                Id = 2,
                Name = "Капитанская дочка",
                Author = authors[1],
                Genres = genres.Where(e=>e.GenreName == "проза" || e.GenreName == "роман" ).ToList(),
                Persons = persons.Take(3).ToList()
            },
            new Book
            {
                Id = 3,
                Name = "Дживс, вы- гений!",
                Author = authors[2],
                Genres = genres.Where(e=>e.GenreName == "проза" || e.GenreName == "роман" ).ToList(),
                Persons = persons.Take(3).ToList()
            },
            new Book
            {
                Id = 4,
                Name = "Фамильная честь Вустеров",
                Author = authors[2],
                 Genres = genres.Where(e=>e.GenreName == "проза" || e.GenreName == "роман" ).ToList(),
                Persons = persons.Take(3).ToList()
            },
            new Book
            {
                Id = 5,
                Name = "Женщина в песках",
                Author = authors[4],
                Genres = genres.Where(e=>e.GenreName == "проза" || e.GenreName == "роман" ).ToList(),
                Persons = persons.Take(2).ToList()
            },
            new Book
            {
                Id = 6,
                Name = "Чужое лицо",
                Author = authors[4],
                Genres = genres.Where(e=>e.GenreName == "проза" || e.GenreName == "роман" ).ToList(),
                Persons = persons.Take(3).ToList()
            },
            new Book
            {
                Id = 7,
                Name = "Сны роботов",
                Author = authors[5],
                Genres = genres.Where(e=>e.GenreName == "научная фантастика" ).ToList(),
                Persons = persons.Take(2).ToList()
            },
        };

    }
}
