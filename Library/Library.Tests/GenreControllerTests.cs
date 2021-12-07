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
    public class GenreControllerTests
    {
        public IMapper mapper;
        public Mock<IUnitOfWork> uow;
        public void TestInit()
        {

            var myProfile = new GenreProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapper = new Mapper(configuration);
            uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IRepository<Genre>>();
            uow.Setup(x => x.GetRepository<Genre>()).Returns(repo.Object);
            uow.Setup(x => x.GetRepository<Genre>().Get(It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(), It.IsAny<string>()))
                                                    .Returns(GetDataGenre());
        }

        [Fact]
        public void GetAllGenresTest()
        {
            // Arrange
            TestInit();
            GenreController genreController = new GenreController(mapper, uow.Object);

            // Act
            List<GenreDto> results = genreController.GetAllGenres();

            // Assert
            Assert.Equal(GetDataGenreDto().Count(), results.Count());

        }

        [Fact]
        public void AddGenreTest()
        {
            // Arrange
            TestInit();
            GenreController genreController = new GenreController(mapper, uow.Object);

            // Act
            var result = genreController.AddGenre(GetDataGenreDto().ElementAt(0));

            // Assert
            Assert.NotNull(((ObjectResult)result.Result).Value);
        }

        [Fact]
        public void GetStaticticTest()
        {
            // Arrange
            TestInit();
            GenreController genreController = new GenreController(mapper, uow.Object);

            // Act
            var result = genreController.GetStatictic();

            // Assert
            Assert.Equal(GetDataStatisticGenreDto().Count(), result.Count());

        }
        private List<Genre> GetDataGenre()
        {
            var genres = new List<Genre>
            {
                new Genre
                {
                    Id = 1,
                    GenreName = "роман",                    
                },
                new Genre
                {
                    Id = 2,
                    GenreName = "научная фантастика",                    
                },
                new Genre
                {
                    Id = 3,
                    GenreName = "проза",
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
                    //Books = books.Where(e => e.Genres.Any(e => e.GenreName == "роман")).ToList()
                },
                new GenreDto
                {
                    Id = 2,
                    GenreName = "научная фантастика",
                    //Books = books.Where(e => e.Genres.Any(e => e.GenreName == "научная фантастика")).ToList()
                },
                new GenreDto
                {
                    Id = 3,
                    GenreName = "проза",
                    //Books = books.Where(e => e.Genres.Any(e => e.GenreName == "проза")).ToList()
                }
            };
            return genres;
        }

        private List<StatisticGenreDto> GetDataStatisticGenreDto()
        {
            var genres = new List<StatisticGenreDto>
            {
                new StatisticGenreDto
                {
                    GenreName = "роман",
                    Count = 0
                },
                new StatisticGenreDto
                {
                    GenreName = "научная фантастика",
                    Count = 0
                },
                new StatisticGenreDto
                {
                    GenreName = "проза",
                    Count = 0
                }
            };

            return genres;
        }
    }
}
