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
    public class GenreControllerTests
    {
        public readonly IMapper mapper;
        public Mock<IUnitOfWork> uow;
        public GenreControllerTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new GenreProfile()));
            mapper = new Mapper(configuration);
            uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IRepository<Genre>>();
            uow.Setup(x => x.GetRepository<Genre>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Genre>().Get(It.IsAny<Expression<Func<Genre, bool>>>(), 
                                                        It.IsAny<Func<IQueryable<Genre>,                                                                IOrderedQueryable<Genre>>>(),
                                                        It.IsAny<string>()))
                                                        .Returns(new TestData().GetDataGenre());
        }

        [Fact]
        public void GetAllGenres_ShouldReturn_CountGenreDto()
        {
            // Arrange
            GenreController genreController = new GenreController(mapper, uow.Object);

            // Act
            List<GenreDto> results = genreController.GetAllGenres();

            // Assert
            Assert.Equal(3, results.Count());
        }

        [Fact]
        public void AddGenre_ShouldReturn_NotNull()
        {
            // Arrange
            GenreController genreController = new GenreController(mapper, uow.Object);
            var fixture = new Fixture();
            var genre = fixture.Build<GenreDto>().With(p => p.Books , new List<BookDto>())
                                    .Create<GenreDto>();
            // Act
            var result = genreController.AddGenre(genre);

            // Assert
            Assert.NotNull(((ObjectResult)result.Result).Value);
        }

        [Fact]
        public void GetStatictic_ShouldReturn_CountStatisticGenreDto()
        {
            // Arrange
            GenreController genreController = new GenreController(mapper, uow.Object);

            // Act
            var result = genreController.GetStatictic();

            // Assert
            Assert.Equal(3, result.Count());

        }
    }
}
