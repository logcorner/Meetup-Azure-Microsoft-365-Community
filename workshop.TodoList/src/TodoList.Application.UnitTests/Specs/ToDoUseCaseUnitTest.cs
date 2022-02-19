using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Application.Exceptions;
using TodoList.Application.Messages;
using TodoList.Domain;
using TodoList.Infrastructure;
using Xunit;
using ArgumentNullException = System.ArgumentNullException;

namespace TodoList.Application.UnitTests.Specs
{
    public class ToDoUseCaseUnitTest
    {
        [Fact]
        public async Task AddToDo_InputIsInvalid_ThrowsArgumentNullException()
        {
            //Arrange
            Mock<IRepository<Todo>> mockTodoRepository = new Mock<IRepository<Todo>>();

            mockTodoRepository.Setup(m => m.Create(It.IsAny<Todo>())).Verifiable();

            ITodoUseCase todoUseCase = new TodoUseCase(mockTodoRepository.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => todoUseCase.AddToDo(null));
        }

        [Fact]
        public async Task AddToDo_InputIsValid_CreateTodo()
        {
            //Arrange
            var toDoForCreationMessage = new ToDoForCreationMessage("this is the title",
                "this is the description", "http://test.com");

            Mock<IRepository<Todo>> mockTodoRepository = new Mock<IRepository<Todo>>();

            mockTodoRepository.Setup(m => m.Create(It.IsAny<Todo>())).Verifiable();

            ITodoUseCase todoUseCase = new TodoUseCase(mockTodoRepository.Object);
            //Act

            await todoUseCase.AddToDo(toDoForCreationMessage);
            //Assert
            mockTodoRepository.Verify(r => r.Create(It.IsAny<Todo>()), Times.Once);
        }

        [Fact]
        public async Task GetTodo__ReturnsAllTodo()
        {
            //Arrange

            var todos = new List<Todo>
            {
                new Todo(1,"title","desc","http://test.com"),
            };

            Mock<IRepository<Todo>> mockTodoRepository = new Mock<IRepository<Todo>>();

            mockTodoRepository.Setup(m => m.Get()).Returns(Task.FromResult(todos.AsEnumerable()));

            ITodoUseCase todoUseCase = new TodoUseCase(mockTodoRepository.Object);
            //Act

            var result = await todoUseCase.GetTodo();
            //Assert
            Assert.Equal(todos, result);
        }

        [Fact]
        public async Task GetTodo_WithId_ReturnsTodo()
        {
            //Arrange
            int todoId = 1;
            var todo = new Todo(todoId, "title", "desc", "http://test.com");

            Mock<IRepository<Todo>> mockTodoRepository = new Mock<IRepository<Todo>>();

            mockTodoRepository.Setup(m => m.Get(todoId)).Returns(
                Task.FromResult(todo));

            ITodoUseCase todoUseCase = new TodoUseCase(mockTodoRepository.Object);
            //Act

            var result = await todoUseCase.GetTodo(todoId);
            //Assert
            Assert.Equal(todo, result);
        }

        [Fact]
        public async Task DeleteToDo_TodoNotFound_ThrowsNotFoundException()
        {
            //Arrange
            Mock<IRepository<Todo>> mockTodoRepository = new Mock<IRepository<Todo>>();

            mockTodoRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult<Todo>(null));

            ITodoUseCase todoUseCase = new TodoUseCase(mockTodoRepository.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => todoUseCase.DeleteToDo(It.IsAny<int>()));
        }

        [Fact]
        public async Task DeleteToDo_TodoExist_ShouldDeleteTodo()
        {
            //Arrange
            var todo = new Todo(1, "title", "desc", "http://test.com");
            Mock<IRepository<Todo>> mockTodoRepository = new Mock<IRepository<Todo>>();

            mockTodoRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult(todo));
            mockTodoRepository.Setup(m => m.Delete(It.IsAny<int>())).Verifiable();

            ITodoUseCase todoUseCase = new TodoUseCase(mockTodoRepository.Object);
            //Act

            await todoUseCase.DeleteToDo(todo.Id);
            //Assert
            mockTodoRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateToDo_InputIsNull_ThrowsNotFoundException()
        {
            //Arrange

            Mock<IRepository<Todo>> mockTodoRepository = new Mock<IRepository<Todo>>();

            mockTodoRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult<Todo>(null));

            ITodoUseCase todoUseCase = new TodoUseCase(mockTodoRepository.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => todoUseCase.UpdateToDo(null));
        }

        [Fact]
        public async Task UpdateToDo_TodoNotFound_ThrowsNotFoundException()
        {
            //Arrange
            var command = new ToDoForUpdateMessage(1, "", "", "");
            Mock<IRepository<Todo>> mockTodoRepository = new Mock<IRepository<Todo>>();

            mockTodoRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult<Todo>(null));

            ITodoUseCase todoUseCase = new TodoUseCase(mockTodoRepository.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => todoUseCase.UpdateToDo(command));
        }

        [Fact]
        public async Task UpdateToDo_TodoExist_ShouldUpdateTodo()
        {
            //Arrange
            var todo = new Todo(1, "title", "desc", "http://test.com");
            Mock<IRepository<Todo>> mockTodoRepository = new Mock<IRepository<Todo>>();
            var command = new ToDoForUpdateMessage(1, "my title", "my desc", "http://tes.com");
            mockTodoRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult(todo));
            mockTodoRepository.Setup(m => m.Update(It.IsAny<Todo>())).Verifiable();

            ITodoUseCase todoUseCase = new TodoUseCase(mockTodoRepository.Object);
            //Act

            await todoUseCase.UpdateToDo(command);
            //Assert
            mockTodoRepository.Verify(r => r.Update(It.IsAny<Todo>()), Times.Once);
        }
    }
}