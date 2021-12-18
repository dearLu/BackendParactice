using AutoMapper;
using Library.Controllers;
using Library.Interfaces;
using Library.Models;
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
    public class HumanDtoControllerTests
    {
        public readonly IMapper mapper;
        public Mock<IUnitOfWork> uow;
        public  HumanDtoControllerTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new PersonProfile()));
            mapper = new Mapper(configuration);
            uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IRepository<Person>>();

            uow.Setup(x => x.GetRepository<Person>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Person>().Get(It.IsAny<Expression<Func<Person, bool>>>(),
                                                        It.IsAny<Func<IQueryable<Person>,
                                                        IOrderedQueryable<Person>>>(), It.IsAny<string>()))
                                                        .Returns(new TestData().GetDataPerson());

        }

        [Fact]
        public void GetAll_ShouldReturn_CountHumanDto()
        {
            // Arrange
            HumanDtoController humanController = new HumanDtoController(mapper, uow.Object);

            // Act
            var result = humanController.GetAll();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetHuman_ShouldReturn_NotNull()
        {
            // Arrange
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
            HumanDtoController humanController = new HumanDtoController(mapper, uow.Object);

            // Act
            var result = humanController.GetById(1);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddHumanDto_ShouldReturn_NotNull() 
        {
            // Arrange
            HumanDtoController humanController = new HumanDtoController(mapper, uow.Object);

            // Act
            var result = humanController.AddHumanDTO(new TestData().GetDataHumanDto().ElementAt(0));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteHumanDto_ShouldReturn_NoContent()
        {
            // Arrange           
            HumanDtoController humanController = new HumanDtoController(mapper, uow.Object);

            // Act
            var result = humanController.DeleteHuman(1);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
