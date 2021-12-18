using Library.Models;
using Library.Tests.Helper;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Library.Tests
{
    public class GenericRepositoryTest
    {
        [Fact]
        public void GetRepository_ShouldReturn_ListBook()
        {
            //Arrange
            var helper = new TestHelper();
            var readyRepo = helper.GetInMemoryReadRepositoryBook();

            // Act
            var result = readyRepo.Get();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Insert_GetById_ShouldReturn_Book()
        {
            //Arrange
            var helper = new TestHelper();
            var readyRepo = helper.GetInMemoryReadRepositoryBook();

            //Act
            readyRepo.Insert(new TestData().GetDataBook().ElementAt(0));
            var result = readyRepo.GetById(1);

            //Assert
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void Insert_DeleteById_ShouldReturn_Null()
        {
            //Arrange
            var helper = new TestHelper();
            var readyRepo = helper.GetInMemoryReadRepositoryBook();

            //Act
            readyRepo.Insert(new TestData().GetDataBook().ElementAt(0));
            readyRepo.Delete(1);
            var result = readyRepo.GetById(1);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldReturn_Null()
        {
            //Arrange
            var helper = new TestHelper();
            var readyRepo = helper.GetInMemoryReadRepositoryBook();
            var book = new TestData().GetDataBook().ElementAt(1);

            //Act
            readyRepo.Insert(book);            
            book.Name = "test";
            readyRepo.Update(book);
            var result = readyRepo.GetById(book.Id);

            //Assert
            Assert.Equal("test", result.Name);
        }
    }
}
