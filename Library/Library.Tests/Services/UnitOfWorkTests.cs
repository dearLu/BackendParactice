using Library.Models;
using Xunit;

namespace Library.Tests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void GetRepository_ShouldReturn_RepositoryBook()
        {
            //Arrange
            var helper = new TestHelper();
            var readyUnitOfWork = helper.GetInMemoryUnitOfWork();

            // Act
            var result = readyUnitOfWork.GetRepository<Book>();

            // Assert
            Assert.NotNull(result);
        }

    }
}
