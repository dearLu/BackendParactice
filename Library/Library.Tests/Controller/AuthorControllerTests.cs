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
    public class AuthorControllerTests
    {
        public readonly IMapper mapper;
        public Mock<IUnitOfWork> uow;
        public  AuthorControllerTests() 
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>(){
                                                                                new AuthorProfile(),
                                                                                new BookProfile(),
                                                                                new GenreProfile()
                                                        }));
            mapper = new Mapper(configuration);
            uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IRepository<Author>>();
            uow.Setup(x => x.GetRepository<Author>()).Returns(repo.Object);
           
            uow.Setup(x => x.GetRepository<Author>().Get(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Func<IQueryable<Author>, IOrderedQueryable<Author>>>(), It.IsAny<string>()))
                                                    .Returns(new TestData().GetDataAuthor());
            var repoBook = new Mock<IRepository<Book>>();
            
            uow.Setup(x => x.GetRepository<Book>()).Returns(repoBook.Object);
            uow.Setup(x => x.GetRepository<Book>().Get(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(), It.IsAny<string>()))
                                                    .Returns(new TestData().GetDataBook());

        }
        [Fact]
        public void GetAuthors_ShouldReturn_CountAuthor()
        {
            // Arrange                     
            AuthorController authorController = new AuthorController(mapper, uow.Object);

            // Act
            List<AuthorDto> results = authorController.GetAuthors();

            // Assert
            Assert.Equal(2, results.Count());

        }

        [Fact]
        public void GetAuthorWithBooks_ShouldReturn_Count() 
        {
            // Arrange           
            AuthorController authorController = new AuthorController(mapper, uow.Object);

            // Act
            List<AuthorBooksGenresDto> results = authorController.GetAuthorWithBooks(new TestData().GetDataAuthorDto().LastOrDefault());

            // Assert
            Assert.Equal(2, results.Count());

        }

        [Fact]        
        public void AddAuthor_ShouldReturn_AuthorWithBook() 
        {
            // Arrange           
            AuthorController authorController = new AuthorController(mapper, uow.Object);
            var fixture = new Fixture();
            var author = fixture.Build<AuthorDto>().With(p => p.Books, new List<BookDto>()).Create<AuthorDto>();

            // Act
            AuthorBooksDto result = authorController.AddAuthor(author);
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteAuthor_ShouldReturn_BadRequest()
        {
            // Arrange
            AuthorController authorController = new AuthorController(mapper, uow.Object);

            // Act
            var actionResult = authorController.DeleteAuthor(new TestData().GetDataAuthorDto().ElementAt(0));

            //Assert
            var badObjectResult = actionResult as BadRequestObjectResult;
            Assert.NotNull(badObjectResult);

            Assert.Equal($"У автора Пушкин есть книги. Нельзя удалить автора, пока есть его книги", badObjectResult.Value);

        }
    }
}
