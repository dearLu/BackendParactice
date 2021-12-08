using AutoMapper;
using Library.Controllers;
using Library.Interfaces;
using Library.Models;
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
    public class HumanDtoControllerTests
    {
        public IMapper mapper;
        public Mock<IUnitOfWork> uow;
        public void TestInit()
        {
            var myProfile = new PersonProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapper = new Mapper(configuration);
            uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IRepository<Person>>();

            uow.Setup(x => x.GetRepository<Person>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Person>().Get(It.IsAny<Expression<Func<Person, bool>>>(),
                                                        It.IsAny<Func<IQueryable<Person>,
                                                        IOrderedQueryable<Person>>>(), It.IsAny<string>()))
                                                        .Returns(GetDataPerson());

        }

        [Fact]
        public void GetAll_ShouldReturn_CountHumanDto()
        {
            // Arrange
            TestInit();
            HumanDtoController humanController = new HumanDtoController(mapper, uow.Object);

            // Act
            var result = humanController.GetAll();

            // Assert
            Assert.Equal(GetDataHumanDto().Count(), result.Count());
        }

        [Fact]
        public void GetHuman_ShouldReturn_NotNull()
        {
            // Arrange
            TestInit();
            HumanDtoController humanController = new HumanDtoController(mapper, uow.Object);

            // Act
            var result = humanController.GetHuman("Александров");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetById_ShouldReturn_Ok()
        {
            // Arrange
            TestInit();
            HumanDtoController humanController = new HumanDtoController(mapper, uow.Object);

            // Act
            var result = humanController.GetById(GetDataHumanDto().ElementAt(0).Id);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddHumanDto_ShouldReturn_NotNull() 
        {
            // Arrange
            TestInit();
            HumanDtoController humanController = new HumanDtoController(mapper, uow.Object);

            // Act
            var result = humanController.AddHumanDTO(GetDataHumanDto().ElementAt(0));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteHumanDto_ShouldReturn_NoContent()
        {
            // Arrange
            TestInit();
            HumanDtoController humanController = new HumanDtoController(mapper, uow.Object);

            // Act
            var result = humanController.DeleteHuman(GetDataHumanDto().ElementAt(0).Id);

            //Assert
            Assert.IsType<NoContentResult>(result);
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
    }
}
