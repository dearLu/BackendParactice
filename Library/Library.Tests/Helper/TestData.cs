using Library.Models;
using Library.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Helper
{
    public class TestData
    {
        public List<BookDto> GetDataBookDto()
        {
            var books = new List<BookDto>()
            {
                new BookDto
                {
                    Id = 1,
                    Title = "Дживс, вы- гений!",
                    Persons = new List<HumanDto>(),
                    Genres = GetDataGenreDto().Take(2).ToList()

                },
                new BookDto
                {
                    Id = 2,
                    Title = "Фамильная честь Вустеров",
                    Genres = GetDataGenreDto().Take(2).ToList()
                }
            };

            return books;
        }
        public List<Genre> GetDataGenre()
        {
            var genres = new List<Genre>
            {
                new Genre
                {
                    Id = 1,
                    GenreName = "роман",
                    Books = new List<Book>()
                },
                new Genre
                {
                    Id = 2,
                    GenreName = "научная фантастика",
                    Books = new List<Book>()
                },
                new Genre
                {
                    Id = 3,
                    GenreName = "проза",
                    Books = new List<Book>()
                }
            };
            return genres;
        }
        public List<Author> GetDataAuthor()
        {
            var authors = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    FirstName = "Александр",
                    LastName = "Пушкин",
                    MiddleName = "Сергеевич"

                },
                new Author
                {
                    Id = 2,
                    FirstName = "Пелам",
                    LastName = "Вудхаус",
                    MiddleName = "Гренвилл"

                }
            };
            return authors;
        }
        public List<AuthorDto> GetDataAuthorDto()
        {
            var authors = new List<AuthorDto>
            {
                new AuthorDto
                {
                    Id = 1,
                    FirstName = "Александр",
                    LastName = "Пушкин",
                    MiddleName = "Сергеевич"

                },
                new AuthorDto
                {
                    Id = 2,
                    FirstName = "Пелам",
                    LastName = "Вудхаус",
                    MiddleName = "Гренвилл",
                    Books = new List<BookDto>()
                    {
                        new BookDto
                        {
                            Id = 1,
                            Title = "Дживс, вы- гений!",
                        },
                        new BookDto
                        {
                            Id = 2,
                            Title = "Фамильная честь Вустеров",
                        }
                    }
                }
            };

            return authors;
        }
        public List<Book> GetDataBook()
        {
            var books = new List<Book>()
            {
                new Book
                {
                    Id = 1,
                    Name = "Дживс, вы- гений!",
                    Author = GetDataAuthor().ElementAt(1)
                },
                new Book
                {
                    Id = 2,
                    Name = "Фамильная честь Вустеров",
                    Author = GetDataAuthor().ElementAt(1)
                }
            };

            return books;
        }
        public List<HumanDto> GetDataHumanDto()
        {
            var persons = new List<HumanDto>()
            {
                new HumanDto
                {
                    Id = 1,
                    Birthday = new DateTime(1960, 1, 1),
                    Name = "Александр",
                    Surname = "Александров",
                    Patronymic = "Александрович",
                },

                new HumanDto
                {
                    Id = 2,
                    Birthday = new DateTime(2000, 05, 07),
                    Name = "Петр",
                    Surname = "Иванов",
                    Patronymic = "Александрович",
                },

                new HumanDto
                {
                    Id = 3,
                    Birthday = new DateTime(1999, 05, 03),
                    Name = "Светлана",
                    Surname = "Александрова",
                    Patronymic = "Сергеевна",
                }
            };
            return persons;
        }
        public List<Person> GetDataPerson()
        {
            var persons = new List<Person>()
            {
                new Person
                {
                    Id = 1,
                    BirthDate = new DateTime(1960, 1, 1),
                    FirstName = "Александр",
                    LastName = "Александров",
                    MiddleName = "Александрович",
                    Books= new List<Book>()
                },

                new Person
                {
                    Id = 2,
                    BirthDate = new DateTime(2000, 05, 07),
                    FirstName = "Петр",
                    LastName = "Иванов",
                    MiddleName = "Александрович",
                },

                new Person
                {
                    Id = 3,
                    BirthDate = new DateTime(1999, 05, 03),
                    FirstName = "Светлана",
                    LastName = "Александрова",
                    MiddleName = "Сергеевна",
                }
            };
            return persons;
        }
        public List<GenreDto> GetDataGenreDto()
        {
            var genres = new List<GenreDto>
            {
                new GenreDto
                {
                    Id = 1,
                    GenreName = "роман",
                    Books = new List<BookDto>()
                },
                new GenreDto
                {
                    Id = 2,
                    GenreName = "научная фантастика",
                    Books = new List<BookDto>()
                },
                new GenreDto
                {
                    Id = 3,
                    GenreName = "проза",
                    Books = new List<BookDto>()
                }
            };
            return genres;
        }
        public List<StatisticGenreDto> GetDataStatisticGenreDto()
        {
            var genres = new List<StatisticGenreDto>
            {
                new StatisticGenreDto
                {
                    GenreName = "роман",
                    Count = 0
                },
                new StatisticGenreDto
                {
                    GenreName = "научная фантастика",
                    Count = 0
                },
                new StatisticGenreDto
                {
                    GenreName = "проза",
                    Count = 0
                }
            };

            return genres;
        }

    }
}
