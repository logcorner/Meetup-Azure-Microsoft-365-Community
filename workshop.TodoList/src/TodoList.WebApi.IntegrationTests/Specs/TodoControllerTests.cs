using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TodoList.Domain;
using TodoList.WebApi.Models.Todo;
using Xunit;

namespace TodoList.WebApi.IntegrationTests.Specs
{
    public class TodoControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private CustomWebApplicationFactory<Startup> _factory;

        public TodoControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/v:1.0/todos/");

            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task AddTodo_WithInvalidInput_ReturnsBadRequest(string title)
        {
            //Arrange
            var toDoForCreationDto = new ToDoForCreationDto
            {
                Title = title
            };

            //Act
            var result = await _client.PostAsJsonAsync("", toDoForCreationDto,
                new JsonSerializerOptions(JsonSerializerDefaults.General));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task AddTodo_WithValidInput_ReturnsOk()
        {
            //Arrange
            var toDoForCreationDto = new ToDoForCreationDto
            {
                Title = "this is the title"
            };

            //Act
            var result = await _client.PostAsJsonAsync("", toDoForCreationDto,
                new JsonSerializerOptions(JsonSerializerDefaults.General));

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task RemoveTodo_WithInvalidInput_ReturnsBadRequest(int id)
        {
            //Arrange

            //Act
            var result = await _client.DeleteAsync($"{id}");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task RemoveTodo_WithValidInput_ReturnsOk()
        {
            //Arrange

            var todo = new Todo(1, "this is the title", "this is the desc", "http://test.com");
            _factory.FakeDataBase.Clear();
            _factory.FakeDataBase.InsertTodoAsync(todo);

            //Act
            var result = await _client.DeleteAsync($"{todo.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task GetTodo_WithInValidInput_ReturnsBadRequest()
        {
            //Arrange
            int id = 0;

            //Act
            var result = await _client.GetAsync($"{id}");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        //[Fact]
        //public async Task GetTodo_WithValidInput_ReturnsOk()
        //{
        //    //Arrange
        //    var todo = new Todo(1, "this is the title", "this is the desc", "http://test.com");
        //    _factory.FakeDataBase.Clear();
        //    _factory.FakeDataBase.InsertTodoAsync(todo);

        //    //Act
        //    var result = await _client.GetFromJsonAsync<List<ToDoInListDto>>("");
        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.NotNull(result.FirstOrDefault());
        //    Assert.Equal(todo.Id, result.FirstOrDefault().Id);
        //    Assert.Equal(todo.Title, result.FirstOrDefault().Title);
        //    Assert.Equal(todo.Description, result.FirstOrDefault().Description);
        //}
    }
}