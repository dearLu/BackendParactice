using AutoMapper;
using Library.Controllers;
using Library.Interfaces;
using Library.Models;
using Library.Models.DTO;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Library.Tests
{
    public class BookControllerTests
    {
        public IMapper mapper;
        public Mock<IUnitOfWork> uow;
        public  BookControllerTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>(){
                                                                                new AuthorProfile(),
                                                                                new BookProfile(),
                                                                                new GenreProfile()
                                                        }));
            mapper = new Mapper(configuration);       
            
            var repo = new Mock<IRepository<Book>>();
            var repoGenre = new Mock<IRepository<Genre>>();

            uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GetRepository<Book>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Book>().Get(It.IsAny<Expression<Func<Book, bool>>>(),
                                                        It.IsAny<Func<IQueryable<Book>, 
                                                        IOrderedQueryable<Book>>>(), It.IsAny<string>()))
                                                        .Returns(GetDataBook());
            
            uow.Setup(x => x.GetRepository<Genre>()).Returns(repoGenre.Object);
            uow.Setup(x => x.GetRepository<Genre>().Get(It.IsAny<Expression<Func<Genre, bool>>>(),
                                                        It.IsAny<Func<IQueryable<Genre>,
                                                        IOrderedQueryable<Genre>>>(), It.IsAny<string>()))
                                                        .Returns(GetDataGenre());
        }

        [Fact]
        public void AddBook_ShouldReturn_NotNull() 
        {
            // Arrange
            BookController bookController = new BookController(mapper, uow.Object);

            // Act
            var result = bookController.AddBook(GetDataBookDto().LastOrDefault());

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteBook_ShouldReturn_Ok()
        {
            // Arrange
            BookController bookController = new BookController(mapper, uow.Object);
            uow.Setup(x => x.GetRepository<Book>().GetById(It.IsAny<object>()))
                                                        .Returns(GetDataBook().ElementAt(0));
            // Act
            var actionResult = bookController.DeleteBook(GetDataBookDto().ElementAt(0).Id);

            //Assert
            Assert.IsType<OkResult>(actionResult);           
        }
        [Fact]
        public void PutBook_ShouldReturn_NotNull()
        {
            // Arrange
            BookController bookController = new BookController(mapper, uow.Object);

            // Act
            var result = bookController.PutBook(GetDataBookDto().ElementAt(0));

            //Assert            
            Assert.NotNull(result);
        }

        [Fact]
        public void GetBooksByAuthor_ShouldReturn_CountAuthor()
        {   
            // Arrange
            TestInit();
            BookController bookController = new BookController(mapper, uow.Object);

            // Act
            var result = bookController.GetBooksByAuthor(GetDataAuthorDto().ElementAt(1));

            // Assert
            Assert.Equal(GetDataAuthorDto().ElementAt(1).Books.Count(), result.Count());
        }

        [Fact]
        public void GetBooksByGenre_ShouldReturn_NotNull() 
        {
            // Arrange
            TestInit();
            BookController bookController = new BookController(mapper, uow.Object);

            // Act
            var result = bookController.GetBooksByGenre(GetDataGenreDto().ElementAt(0));

            // Assert
            Assert.NotNull( result);
        }
        private List<Book> GetDataBook()
        {
            var books = new List<Book>()
            {
                new Book
                {
                    Id = 1,
                    Name = "Дживс, вы- гений!",
                    Persons = new List<Person>(),
                    Genres = GetDataGenre().Take(2).ToList()
                },
                new Book
                {
                    Id = 2,
                    Name = "Фамильная честь Вустеров",
                    Genres = GetDataGenre().Take(2).ToList()
                }
            };

            return books;
        }
        private List<BookDto> GetDataBookDto()
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
        private List<Genre> GetDataGenre()
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
        private List<GenreDto> GetDataGenreDto()
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
        private List<AuthorDto> GetDataAuthorDto()
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
    }
}
