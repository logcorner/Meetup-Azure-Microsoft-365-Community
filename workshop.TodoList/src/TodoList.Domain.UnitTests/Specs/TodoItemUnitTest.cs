using Moq;
using System;
using Xunit;

namespace TodoList.Domain.UnitTests.Specs
{
    public class TodoItemUnitTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TodoItem_TitleIsNullOrEmpty_ThrowsArgumentNullException(string title)
        {
            //Arrange
            string description = "desc";
            var status = Status.New;

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => new TodoItem(It.IsAny<int>(), title, description, status, ""));
        }
    }
}