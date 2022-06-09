using TodoList.Domain.Exceptions;
using Xunit;

namespace TodoList.Domain.UnitTests.Specs
{
    public class DomainExceptionUnitTest
    {
        [Fact]
        public void ShouldSetErrorCode()
        {
            //Arrange
            int errorCode = 1;
            string message = "this is an error message";

            //Act
            var domainException = new DomainException(errorCode, message);

            //Assert
            Assert.Equal(errorCode, domainException.ErrorCode);

            Assert.Equal(message, domainException.Message);
        }
    }
}