using TodoList.Application.Exceptions;
using Xunit;

namespace TodoList.Application.UnitTests.Specs
{
    public class ApplicationExceptionUnitTest
    {
        [Fact]
        public void ShouldSetErrorCode()
        {
            //Arrange
            int errorCode = 1;
            string message = "this is an error message";

            //Act
            var domainException = new ApplicationException(errorCode, message);

            //Assert
            Assert.Equal(errorCode, domainException.ErrorCode);

            Assert.Equal(message, domainException.Message);
        }
    }
}