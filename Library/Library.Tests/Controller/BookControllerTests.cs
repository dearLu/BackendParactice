using AutoFixture;
using AutoMapper;
using Library.Controllers;
using Library.Interfaces;
using Library.Models;
using Library.Models.DTO;
using Library.Services;
using Library.Tests.Helper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Library.Tests
{
    public class BookControllerTests
    {
        public readonly IMapper mapper;
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
                                                        .Returns(new TestData().GetDataGenre());
        }

        [Fact]
        public void AddBook_ShouldReturn_NotNull() 
        {
            // Arrange
            BookController bookController = new BookController(mapper, uow.Object);
            var fixture = new Fixture();
            var book = fixture.Build<BookDto>().With(p => p.Genres, new List<GenreDto>())
                                                .With(p => p.Persons, new List<HumanDto>())
                                                .With(p => p.Author, new AuthorDto())
                                                .Create<BookDto>();
            // Act
            var result = bookController.AddBook(book);

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
            var actionResult = bookController.DeleteBook(1);

            //Assert
            Assert.IsType<OkResult>(actionResult);           
        }
        [Fact]
        public void PutBook_ShouldReturn_NotNull()
        {
            // Arrange
            BookController bookController = new BookController(mapper, uow.Object);

            // Act
            var result = bookController.PutBook(new TestData().GetDataBookDto().ElementAt(0));

            //Assert            
            Assert.NotNull(result);
        }

        [Fact]
        public void GetBooksByAuthor_ShouldReturn_CountAuthor()
        {   
            // Arrange
            BookController bookController = new BookController(mapper, uow.Object);

            // Act
            var result = bookController.GetBooksByAuthor(new TestData().GetDataAuthorDto().ElementAt(1));

            // Assert
            Assert.Equal(new TestData().GetDataAuthorDto().ElementAt(1).Books.Count(), result.Count());
        }

        [Fact]
        public void GetBooksByGenre_ShouldReturn_NotNull() 
        {
            // Arrange
            BookController bookController = new BookController(mapper, uow.Object);

            // Act
            var result = bookController.GetBooksByGenre(new TestData().GetDataGenreDto().ElementAt(0));

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
                    Genres = new TestData().GetDataGenre().Take(2).ToList()
                },
                new Book
                {
                    Id = 2,
                    Name = "Фамильная честь Вустеров",
                    Genres = new TestData().GetDataGenre().Take(2).ToList()
                }
            };

            return books;
        }
    }
}
