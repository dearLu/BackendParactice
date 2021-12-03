using AutoMapper;
using Library.Controllers;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Library.Tests
{
    public class AuthorControllerTests
    {
        #region Property  
        public Mock<IRepository<Author>> mockRepo = new Mock<IRepository<Author>>();
        public Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
        public Mock<IMapper> mockMapper = new Mock<IMapper>();
        #endregion

        [Fact]
        public async void GetAuthors()
        {
            // Arrange

            mock.Setup(repo => repo.GetRepository<Author>().Get(null, null, null)).Returns(GetTestAuthors());
            AuthorController authorController = new AuthorController(mockMapper.Object, mock.Object);

            // Act
            var result = authorController.GetAuthors();

            // Assert

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Author>>(viewResult.Model);
            Assert.Equal(GetTestAuthors().Count, model.Count());
        }

        public List<Author> GetTestAuthors()
        {
           
            return DataTest.authors;
        }

    }
}
