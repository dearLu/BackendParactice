using AutoMapper;
using Library.Controllers;
using Library.Interfaces;
using Library.Models;
using Library.Models.DTO;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class AuthorControllerTests
    {
        [Fact]
        public void GetAuthorsTest()
        {
            // Arrange
            var myProfile = new AuthorProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper  = new Mapper(configuration);
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IRepository<Author>>();          
            uow.Setup(x => x.GetRepository<Author>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Author>().Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>()))
                                                    .Returns(GetDataAuthor());
            AuthorController authorController = new AuthorController(mapper, uow.Object);

            // Act
            List<AuthorDto> results = authorController.GetAuthors();

            // Assert
            Assert.Equal(GetDataAuthorDto().Count(), results.Count());

        }

        [Fact]
        public void GetAuthorWithBooksTest() 
        {
            // Arrange
            var myProfile = new AuthorProfile();
            var myProfile2 = new BookProfile();
            var myProfile3 = new GenreProfile();
            var listProfilies = new List<Profile>() { myProfile, myProfile2, myProfile3 };
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(listProfilies));
            IMapper mapper = new Mapper(configuration);
            var repo = new Mock<IRepository<Book>>();
            Mock<IUnitOfWork>  uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GetRepository<Book>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Book>().Get(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(), It.IsAny<string>()))
                                                    .Returns(GetDataBook());
            AuthorController authorController = new AuthorController(mapper, uow.Object);

            // Act
            List<AuthorBooksGenresDto> results = authorController.GetAuthorWithBooks(GetDataAuthorDto().LastOrDefault());

            // Assert
            Assert.Equal(GetAuthorBooksGenresDto().Count(), results.Count());

        }

        [Fact]        
        public void AddAuthorTest() 
        {
            // Arrange
            var myProfile = new AuthorProfile();
            var myProfile2 = new BookProfile();            
            var listProfilies = new List<Profile>() { myProfile, myProfile2 };
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(listProfilies));
            IMapper mapper = new Mapper(configuration);
            var repo = new Mock<IRepository<Author>>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GetRepository<Author>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Author>().Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>()))
                                                    .Returns(GetDataAuthor());
            AuthorController authorController = new AuthorController(mapper, uow.Object);

            // Act
            AuthorBooksDto result = authorController.AddAuthor(GetDataAuthorDto().ElementAt(1));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteAuthorTest_BadRequest()
        {
            // Arrange
            var myProfile = new AuthorProfile();
            var myProfile2 = new BookProfile();
            var listProfilies = new List<Profile>() { myProfile, myProfile2 };
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(listProfilies));
            IMapper mapper = new Mapper(configuration);
            var repo = new Mock<IRepository<Author>>();
            var repoBook = new Mock<IRepository<Book>>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GetRepository<Author>()).Returns(repo.Object);
            
            uow.Setup(x => x.GetRepository<Author>().Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>()))
                                                    .Returns(GetDataAuthor());
            uow.Setup(x => x.GetRepository<Book>()).Returns(repoBook.Object);
            uow.Setup(x => x.GetRepository<Book>().Get(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(), It.IsAny<string>()))
                                        .Returns(GetDataBook());
            AuthorController authorController = new AuthorController(mapper, uow.Object);

            // Act
            var actionResult = authorController.DeleteAuthor(GetDataAuthorDto().ElementAt(0));

            //Assert
            var badObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badObjectResult);

            Assert.Equal($"У автора Пушкин есть книги. Нельзя удалить автора, пока есть его книги", badObjectResult.Value);

        }
        private List<Author> GetDataAuthor()
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
        private List<Book> GetDataBook() 
        {
            var  books = new List<Book>()
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
        private List<BookDto> GetDataBookDto()
        {
            var books = new List<BookDto>()
            {
                new BookDto
                {
                    Id = 1,
                    Title = "Дживс, вы- гений!",
                    Author = GetDataAuthorDto().ElementAt(1),
                    Genres = new List<GenreDto>(),
                    Persons = new List<HumanDto>(),

                },
                new BookDto
                {
                    Id = 2,
                    Title = "Фамильная честь Вустеров",
                    Author = GetDataAuthorDto().ElementAt(1),
                    Genres = new List<GenreDto>(),
                    Persons = new List<HumanDto>(),
                }
            };

            return books;
        }
        private List<AuthorBooksGenresDto> GetAuthorBooksGenresDto() 
        {
            var books = new List<AuthorBooksGenresDto>()
            {
                new AuthorBooksGenresDto
                {

                    Author = GetDataAuthorDto().ElementAt(1),
                    Book = GetDataBookDto().ElementAt(0)

                },
                new AuthorBooksGenresDto
                {
                    Author = GetDataAuthorDto().ElementAt(1),
                    Book = GetDataBookDto().ElementAt(1)

                }
            };

            return books;

        }

    }
}
