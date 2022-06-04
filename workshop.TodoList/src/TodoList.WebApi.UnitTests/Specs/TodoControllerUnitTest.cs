using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TodoList.Application;
using TodoList.Application.Messages;
using TodoList.Domain;
using TodoList.SharedKernel.Repository;
using TodoList.WebApi.Controllers;
using TodoList.WebApi.Models.Todo;
using Xunit;

namespace TodoList.WebApi.UnitTests.Specs
{
    public class TodoControllerUnitTest
    {
        [Fact]
        public async Task Get__ReturnsAllTodo()
        {
            //Arrange
            var todos = new List<Todo>
            {
                new Todo(1,"title","desc","http://test.com"),
            }.AsEnumerable();

            var moqTodoUseCase = new Mock<ITodoUseCase>();
            moqTodoUseCase.Setup(x => x.GetTodo())
                .Returns(Task.FromResult(todos));

            var sut = new TodoController(moqTodoUseCase.Object);

            //Act
            IActionResult result = await sut.Get();

            //Assert
            var okResult = result as OkObjectResult;
            var todoResult = (IEnumerable<ToDoInListDto>)okResult?.Value;
            Assert.NotNull(okResult);
            Assert.NotNull(todoResult);
            //Assert.Collection<ToDoInListDto>(todos.Select(result => new ToDoInListDto
            //{
            //    Id = result.Id,
            //    Title = result.Title,
            //    Description = result.Description,
            //    Status = result.Status,
            //    ImageUrl = result.ImageUrl,
            //    Tasks = result.Tasks.ToList()
            //}), todoResult);
        }

        [Fact]
        public async Task Get_InputIsInvalid__ThrowsArgumentNullException()
        {
            //Arrange
            int id = 1;
            var todo = new Todo(id, "title", "desc", "http://test.com");

            var moqTodoUseCase = new Mock<ITodoUseCase>();
            moqTodoUseCase.Setup(x => x.GetTodo(id))
                .Returns(Task.FromResult(todo));

            var sut = new TodoController(moqTodoUseCase.Object);

            //Act
            IActionResult result = await sut.Get(id);

            //Assert
            var okResult = result as OkObjectResult;
            var todoResult = (ToDoInListDto)okResult?.Value;
            Assert.NotNull(okResult);
            Assert.NotNull(todoResult);
        }

        [Fact]
        public async Task Delete_InputIsInvalid__ThrowsArgumentNullException()
        {
            //Arrange
            int id = 0;

            var moqTodoUseCase = new Mock<ITodoUseCase>();

            var sut = new TodoController(moqTodoUseCase.Object);

            //Act
            IActionResult result = await sut.Delete(id);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_InputIsValid_ShouldReturnOK()
        {
            //Arrange
            int id = 1;

            var moqTodoUseCase = new Mock<ITodoUseCase>();
            moqTodoUseCase.Setup(m => m.DeleteToDo(It.IsAny<int>())).Verifiable();

            var sut = new TodoController(moqTodoUseCase.Object);

            //Act
            IActionResult result = await sut.Delete(id);

            //Assert
            Assert.IsType<OkResult>(result);
            moqTodoUseCase.Verify(r => r.DeleteToDo(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Update_InputIsInvalid__ThrowsArgumentNullException()
        {
            //Arrange
            var input = new ToDoForUpdateDto
            {
                Id = 0,
                Title = "This is the title",
                Description = "This is the title",
                ImageUrl = "http://www.test.com"
            };

            var moqTodoUseCase = new Mock<ITodoUseCase>();

            var sut = new TodoController(moqTodoUseCase.Object);

            //Act
            IActionResult result = await sut.Update(input);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_InputIsValid_ShouldReturnOK()
        {
            //Arrange
            var input = new ToDoForUpdateDto
            {
                Id = 1,
                Title = "This is the title",
                Description = "This is the title",
                ImageUrl = "http://www.test.com"
            };

            var moqTodoUseCase = new Mock<ITodoUseCase>();
            moqTodoUseCase.Setup(m => m.UpdateToDo(It.IsAny<ToDoForUpdateMessage>())).Verifiable();

            var sut = new TodoController(moqTodoUseCase.Object);

            //Act
            IActionResult result = await sut.Update(input);

            //Assert
            Assert.IsType<OkResult>(result);
            moqTodoUseCase.Verify(r => r.UpdateToDo(It.IsAny<ToDoForUpdateMessage>()), Times.Once);
        }

        [Fact]
        public async Task Get_PaginatedParameterIsInvalid__ShouldReturnBadRequest()
        {
            //Arrange

            var moqTodoUseCase = new Mock<ITodoUseCase>();

            var sut = new TodoController(moqTodoUseCase.Object);

            //Act
            IActionResult result = await sut.Get(new PaginationParameterDto());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Get_PaginatedParameterIsValid__ShouldReturnOK()
        {
            //Arrange
            var response = new PaginationItems<Todo>(new List<Todo>(), 1, 1, 10);
            var httpContext = new DefaultHttpContext();
            var moqTodoUseCase = new Mock<ITodoUseCase>();
            moqTodoUseCase.Setup(m => m.GetTodo(It.IsAny<PaginationParameter>())).Returns(Task.FromResult(response));
            var sut = new TodoController(moqTodoUseCase.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                }
            };

            //Act
            var paginationParameter = new PaginationParameterDto
            {
                PageNumber = 1,
                PageSize = 10
            };
            IActionResult result = await sut.Get(paginationParameter);

            //Assert
            Assert.IsType<OkObjectResult>(result);

            Assert.True(sut.Response.Headers.Keys.Contains("X-Pagination"));

            var value = JsonSerializer.Serialize(
                 new
                 {
                     response.TotalCount,
                     response.TotalPages,
                     response.PageSize,
                     response.CurrentPage,
                     response.HasNext,
                     response.HasPrevious
                 });
            Assert.Equal(value, sut.Response.Headers["X-Pagination"]);
        }
    }
}