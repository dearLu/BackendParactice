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
    public class BookControllerDtoTests
    {
        public IMapper mapper;
        public Mock<IUnitOfWork> uow;
        public  BookControllerDtoTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>(){
                                                                                new AuthorProfile(),
                                                                                new BookProfile(),
                                                                                new GenreProfile()
                                                        }));
            mapper = new Mapper(configuration);
            uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IRepository<Book>>();

            uow.Setup(x => x.GetRepository<Book>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Book>().Get(It.IsAny<Expression<Func<Book, bool>>>(),
                                                        It.IsAny<Func<IQueryable<Book>,
                                                        IOrderedQueryable<Book>>>(), It.IsAny<string>()))
                                                        .Returns(GetDataBook());
        }

        [Fact]
        public void GetAllBook_ShouldReturn_CountBook()
        {
            // Arrange
            BookDtoController bookController = new BookDtoController(mapper, uow.Object);

            // Act
            var result = bookController.GetAllBook();

            // Assert
            Assert.Equal(GetDataBookDto().Count(), result.Count());
        }

        [Fact]
        public void GetFilterBooksetAllBook_ShouldReturn_CountBook()
        {
            // Arrange
            BookDtoController bookController = new BookDtoController(mapper, uow.Object);

            // Act
            var result = bookController.GetFilterBooks();

            // Assert
            Assert.Equal(GetDataBookDto().Count(), result.Count());
        }

        [Fact]
        public void GetBookByAuthor_ShouldReturn_NotNull()
        {
            // Arrange
            BookDtoController bookController = new BookDtoController(mapper, uow.Object);
            uow.Setup(x => x.GetRepository<Book>().GetById(It.IsAny<object>()))
                                            .Returns(GetDataBook().ElementAt(1));
            // Act
            var result = bookController.GetBookByAuthor(GetDataAuthor().ElementAt(1).Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetById_ShouldReturn_Ok() 
        {
            // Arrange
            BookDtoController bookController = new BookDtoController(mapper, uow.Object);
            // Act
            var actionResult = bookController.GetById(GetDataBook().ElementAt(1).Id);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public void AddBookDTO_ShouldReturn_NotNull()
        {
            // Arrange
            BookDtoController bookController = new BookDtoController(mapper, uow.Object);

            // Act
            var result = bookController.AddBookDTO(GetDataBookDto().ElementAt(1));

            //Assert
            Assert.NotNull(result);
        }


        [Fact]
        public void DeleteBook_ShouldReturn_NoContent()
        {
            // Arrange
            BookDtoController bookController = new BookDtoController(mapper, uow.Object);
            uow.Setup(x => x.GetRepository<Book>().GetById(It.IsAny<object>()))
                                .Returns(GetDataBook().ElementAt(1));
            // Act
            var result = bookController.DeleteBook(GetDataBook().ElementAt(1).Id);

            //Assert
            Assert.IsType<NoContentResult>(result);
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
                },
                new Book
                {
                    Id = 2,
                    Name = "Фамильная честь Вустеров",
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
                },
                new BookDto
                {
                    Id = 2,
                    Title = "Фамильная честь Вустеров",
                }
            };

            return books;
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
    }
}
