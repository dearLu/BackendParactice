using Library.Interfaces;
using Library.Models;
using Library.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            readyRepo.Insert(GetDataBook().ElementAt(0));
            var result = readyRepo.GetById(GetDataBook().ElementAt(0).Id);
            //Assert
            Assert.Equal(GetDataBook().ElementAt(0).Id, result.Id);
        }

        [Fact]
        public void Insert_DeleteById_ShouldReturn_Null()
        {
            //Arrange
            var helper = new TestHelper();
            var readyRepo = helper.GetInMemoryReadRepositoryBook();

            //Act
            readyRepo.Insert(GetDataBook().ElementAt(0));
            readyRepo.Delete(GetDataBook().ElementAt(0).Id);
            var result = readyRepo.GetById(GetDataBook().ElementAt(0).Id);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldReturn_Null()
        {
            //Arrange
            var helper = new TestHelper();
            var readyRepo = helper.GetInMemoryReadRepositoryBook();
            var book = GetDataBook().ElementAt(3);
            //Act
            readyRepo.Insert(book);            
            book.Name = "test";
            readyRepo.Update(book);
            var result = readyRepo.GetById(book.Id);

            //Assert
            Assert.Equal("test", result.Name);
        }
        private List<Book> GetDataBook()
        {
            var books = new List<Book>()
            {
                new Book
                {
                    Id = 1,
                    Name = "Дживс, вы- гений!",
                    Persons = new List<Person>()
                },
                new Book
                {
                    Id = 2,
                    Name = "Фамильная честь Вустеров"
                },
                new Book
                {
                    Id = 3,
                    Name = "Фамильная честь Вустеров"
                },
                 new Book
                {
                    Id = 3,
                    Name = "Фамильная честь Вустеров"
                }
            };

            return books;
        }
    }
}
