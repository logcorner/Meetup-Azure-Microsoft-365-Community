using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TodoList.Domain.UnitTests.Specs
{
    public class TodoUnitTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Todo_TitleIsNullOrEmpty_ThrowsArgumentNullException(string title)
        {
            //Arrange
            string description = "desc";
            string imageUrl = "imageUrl";

            //Act
            //Assert
            Assert.Throws<Exceptions.ArgumentNullException>(() => new Todo(It.IsAny<int>(), title, description, imageUrl));
        }

      

        [Fact]
        public void Todo_IsInvalidInput_CreateInstance()
        {
            //Arrange
            string title = "this is the title";
            string description = "desc";
            string imageUrl = "http://test.com";

            //Act
            var todo = new Todo(It.IsAny<int>(), title, description, imageUrl);

            //Assert
            Assert.Equal(title, todo.Title);
            Assert.Equal(description, todo.Description);
            Assert.Equal(Status.New, todo.Status);
            Assert.Equal(imageUrl, todo.ImageUrl);
        }

        [Fact]
        public void AddTasks_ToDoItemsIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            string title = "this is the title";
            string description = "desc";
            string imageUrl = "http://test.com";

            var todo = new Todo(It.IsAny<int>(), title, description, imageUrl);

            //Act
            //Assert
            Assert.Throws<Exceptions.ArgumentNullException>(() => todo.AddTasks(null));
        }

        [Fact]
        public void AddTasks_ToDoItemsIsNoNull_ShouldAddToDoItemsToListOfTasks()
        {
            //Arrange
            string title = "this is the title";
            string description = "desc";
            string imageUrl = "http://test.com";

            var todo = new Todo(It.IsAny<int>(), title, description, imageUrl);
            var todoItems = new List<TodoItem>
            {
                new TodoItem(1,"this is the title","this is the desc",Status.New, "this is the assignee")
            };

            //Act
            todo.AddTasks(todoItems);

            //Assert
            Assert.Equal(todoItems, todo.Tasks);
            Assert.Equal(todoItems.FirstOrDefault()?.Id, todo.Tasks.FirstOrDefault()?.Id);
            Assert.Equal(todoItems.FirstOrDefault()?.Title, todo.Tasks.FirstOrDefault()?.Title);
            Assert.Equal(todoItems.FirstOrDefault()?.Description, todo.Tasks.FirstOrDefault()?.Description);
            Assert.Equal(todoItems.FirstOrDefault()?.Status, todo.Tasks.FirstOrDefault()?.Status);
            Assert.Equal(todoItems.FirstOrDefault()?.AssignedTo, todo.Tasks.FirstOrDefault()?.AssignedTo);
        }

        [Fact]
        public void Todo_ImageUrlIsInvalid_ThrowsArgumentNullException()
        {
            string title = "this is the title";
            string description = "desc";
            string imageUrl = "http://test.com";

            var todo = new Todo(It.IsAny<int>(), title, description, imageUrl);

            //Act
            //Assert
            Assert.Throws<Exceptions.ArgumentNullException>(() => todo.RemoveTasks(null));
        }

        [Fact]
        public void RemoveTasks_ToDoItemsIsNoNull_ShouldAddToDoItemsToListOfTasks()
        {
            //Arrange
            string title = "this is the title";
            string description = "desc";
            string imageUrl = "http://test.com";

            var todo = new Todo(It.IsAny<int>(), title, description, imageUrl);
            var todoItems = new List<TodoItem>
            {
                new TodoItem(0x1,"this is the title","this is the desc",Status.New,"this is the assignee")
            };

            //Act
            todo.AddTasks(todoItems);
            todo.RemoveTasks(todoItems);

            //Assert
            Assert.False(todo.Tasks.Any());
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "", " ")]
        [InlineData("", "", "")]
        public void Update_AllParametersAreNull_ThrowsArgumentNullException(string title, string description, string imageUrl)
        {
            //Arrange
        

            //Act
            var todo = new Todo(1, "this is the title", "this is the description",
                "http://test.com");
            //Assert
            Assert.Throws<Exceptions.ArgumentNullException>(() => todo.Update(title, description, imageUrl));
        }

        [Fact]
        public void Update_ImageUrlIsInalid_ThrowsArgumentNullException()
        {
            //Arrange
            string title = "this is the title";
            string description = "desc";
            string imageUrl = "imageUrl";
            //Act
            var todo = new Todo(1, "this is the title", "this is the description",
                "http://test");
            //Assert
            Assert.Throws<Exceptions.ArgumentNullException>(() => todo.Update(title, description, imageUrl));
        }

        [Fact]
        public void Update_InputIsValid_ShouldUpdateTodo()
        {
            //Arrange
            string title = "this is the title 1";
            string description = "desc 1";
            string imageUrl = "http://test1.com";

            //Act
            var todo = new Todo(1, "this is the title", "this is the description",
                "http://test.com");
            todo.Update(title, description, imageUrl);

            //Assert
            Assert.Equal(title, todo.Title);
            Assert.Equal(description, todo.Description);
            Assert.Equal(imageUrl, todo.ImageUrl);
        }
    }
}