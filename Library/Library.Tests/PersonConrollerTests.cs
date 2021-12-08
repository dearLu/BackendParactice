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
    public class PersonConrollerTests
    {
        public IMapper mapper;
        public Mock<IUnitOfWork> uow;
        public void TestInit()
        {
            var myProfile = new PersonProfile();
            var myProfile2 = new BookProfile();
            var listProfilies = new List<Profile>() { myProfile, myProfile2};
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(listProfilies));
            mapper = new Mapper(configuration);

            var repo = new Mock<IRepository<Person>>();
            var repoBook = new Mock<IRepository<Book>>();

            uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GetRepository<Person>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Person>().Get(It.IsAny<Expression<Func<Person, bool>>>(),
                                                        It.IsAny<Func<IQueryable<Person>,
                                                        IOrderedQueryable<Person>>>(), It.IsAny<string>()))
                                                        .Returns(GetDataPerson());

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
            TestInit();
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var result = personController.AddPerson(GetDataHumanDto().ElementAt(0));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void UpdatePerson_ShouldReturn_NotNull()
        {
            // Arrange
            TestInit();
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var result = personController.UpdatePerson(GetDataHumanDto().ElementAt(0));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DeletePerson_ShouldReturn_Ok() 
        {
            // Arrange
            TestInit();
            PersonController personController = new PersonController(mapper, uow.Object);
            uow.Setup(x => x.GetRepository<Person>().GetById(It.IsAny<object>()))
                                                        .Returns(GetDataPerson().ElementAt(0));
            // Act
            var actionResult = personController.DeletePerson(GetDataHumanDto().ElementAt(0).Id);

            //Assert
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public void DeletePersonByName_ShouldReturn_Ok()
        {
            // Arrange
            TestInit();
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var actionResult = personController.DeletePersonByName(GetDataHumanDto().ElementAt(0));

            //Assert
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public void GetListBook_ShouldReturn_CountAuthorBooksGenresDto()
        {
            // Arrange
            TestInit();
            List<AuthorBooksGenresDto> forCheck = new List<AuthorBooksGenresDto>()
            {
                new AuthorBooksGenresDto
                {
                    Author = new AuthorDto(),
                    Book =  new BookDto
                    {
                        Id = 1,
                        Title = "Дживс, вы- гений!",

                    },
                    Genre =new List<GenreDto>()
                },
                new AuthorBooksGenresDto
                {
                    Author = new AuthorDto(),
                    Book = new BookDto
                    {
                        Id = 2,
                        Title = "Фамильная честь Вустеров",

                    },
                    Genre =new List<GenreDto>()
                }
            };
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var actionResult = personController.GetListBook(GetDataHumanDto().ElementAt(0).Id);
           
            //Assert
            Assert.Equal(forCheck.Count,actionResult.Count);
        }

        [Fact]
        public void PutBookForPerson_ShouldReturn_NotNull()
        {
            // Arrange
            TestInit();
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var actionResult = personController.PutBookForPerson(GetDataPerson().ElementAt(0).Id, GetDataBookDto().ElementAt(0));

            //Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void ReturnBook_ShouldReturn_NotNullt()
        {
            // Arrange
            TestInit();
            PersonController personController = new PersonController(mapper, uow.Object);

            // Act
            var actionResult = personController.ReturnBook(GetDataPerson().ElementAt(0).Id, GetDataBookDto().ElementAt(0));

            //Assert
            Assert.NotNull(actionResult);
        }
        private List<Person> GetDataPerson()
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

        private List<HumanDto> GetDataHumanDto()
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
        private List<Book> GetDataBook()
        {
            var books = new List<Book>()
            {
                new Book
                {
                    Id = 1,
                    Name = "Дживс, вы- гений!",
                    Persons = GetDataPerson().Take(1).ToList(),
                },
                new Book
                {
                    Id = 2,
                    Name = "Фамильная честь Вустеров",
                    Persons = GetDataPerson().Take(2).ToList(),
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
