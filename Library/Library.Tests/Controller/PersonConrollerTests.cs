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
    public class PersonConrollerTests
    {
        public readonly IMapper mapper;
        public Mock<IUnitOfWork> uow;
        public PersonConrollerTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>(){
                                                                                new PersonProfile(),
                                                                                new BookProfile()
                                                                                }));
                                                                                                                           
            mapper = new Mapper(configuration);

            var repo = new Mock<IRepository<Person>>();
            var repoBook = new Mock<IRepository<Book>>();

            uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GetRepository<Person>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Person>().Get(It.IsAny<Expression<Func<Person, bool>>>(),
                                                        It.IsAny<Func<IQueryable<Person>,
                                                        IOrderedQueryable<Person>>>(), It.IsAny<string>()))
                                                        .Returns(new TestData().GetDataPerson());

            uow.Setup(x => x.GetRepository<Book>()).Returns(repoBook.Object);
            uow.Setup(x => x.GetRepository<Book>().Get(It.IsAny<Expression<Func<Book, bool>>>(),
                                                        It.IsAny<Func<IQueryable<Book>,
                                                        IOrderedQueryable<Book>>>(), It.IsAny<string>()))
                                                        .Returns(GetDataBook());
        }

        [Fact]
        public void AddPerson_ShouldReturn_NotNull() 
        {
            // Arrange
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var result = personController.AddPerson(new TestData().GetDataHumanDto().ElementAt(0));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void UpdatePerson_ShouldReturn_NotNull()
        {
            // Arrange
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var result = personController.UpdatePerson(new TestData().GetDataHumanDto().ElementAt(0));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DeletePerson_ShouldReturn_Ok() 
        {
            // Arrange
            PersonController personController = new PersonController(mapper, uow.Object);
            uow.Setup(x => x.GetRepository<Person>().GetById(It.IsAny<object>()))
                                                        .Returns(new TestData().GetDataPerson().ElementAt(0));
            // Act
            var actionResult = personController.DeletePerson(1);

            //Assert
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public void DeletePersonByName_ShouldReturn_Ok()
        {
            // Arrange
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var actionResult = personController.DeletePersonByName(new TestData().GetDataHumanDto().ElementAt(0));

            //Assert
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public void GetListBook_ShouldReturn_CountAuthorBooksGenresDto()
        {
            // Arrange
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var actionResult = personController.GetListBook(1);
           
            //Assert
            Assert.Equal(2,actionResult.Count);
        }

        [Fact]
        public void PutBookForPerson_ShouldReturn_NotNull()
        {
            // Arrange
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var actionResult = personController.PutBookForPerson(1, GetDataBookDto().ElementAt(0));

            //Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void ReturnBook_ShouldReturn_NotNullt()
        {
            // Arrange
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var actionResult = personController.ReturnBook(1, GetDataBookDto().ElementAt(0));

            //Assert
            Assert.NotNull(actionResult);
        }
        private List<Book> GetDataBook()
        {
            var books = new List<Book>()
            {
                new Book
                {
                    Id = 1,
                    Name = "Дживс, вы- гений!",
                    Persons = new TestData().GetDataPerson().Take(1).ToList(),
                },
                new Book
                {
                    Id = 2,
                    Name = "Фамильная честь Вустеров",
                    Persons = new TestData().GetDataPerson().Take(2).ToList(),
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
                },
                new BookDto
                {
                    Id = 2,
                    Title = "Фамильная честь Вустеров",
                }
            };

            return books;
        }
    }
}
