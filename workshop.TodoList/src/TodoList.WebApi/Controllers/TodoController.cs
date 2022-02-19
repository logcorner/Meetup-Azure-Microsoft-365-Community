using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TodoList.Application;
using TodoList.Application.Messages;
using TodoList.Domain;
using TodoList.SharedKernel.Repository;
using TodoList.WebApi.Models.Todo;

namespace TodoList.WebApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private ITodoUseCase TodoUseCase { get; }

        public TodoController(ITodoUseCase todoUseCase)
        {
            TodoUseCase = todoUseCase;
        }

        /// <summary>
        /// Get list of all todoes
        /// </summary>
        /// <returns>List of todoes</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDoInListDto))]
        public async Task<IActionResult> Get()
        {
            var todos = await TodoUseCase.GetTodo();

            return Ok(todos?.Select(result => new ToDoInListDto
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                Status = result.Status,
                ImageUrl = result.ImageUrl,
                Tasks = result.Tasks.Select(t => new TodoItemInListDto()).ToList()
            }));
        }

        /// <summary>
        /// Get List of paginated todo
        /// </summary>
        /// <param name="paginationParameter">Parameter of pagination</param>
        /// <returns>Current page</returns>
        [HttpGet("Pagination")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationItems<Todo>))]
        public async Task<IActionResult> Get([FromQuery] PaginationParameterDto paginationParameter)
        {
            if (paginationParameter.PageSize <= 0)
            {
                return BadRequest($"invalid PageSize {paginationParameter.PageSize}");
            }
            if (paginationParameter.PageNumber <= 0)
            {
                return BadRequest($"invalid PageNumber {paginationParameter.PageNumber}");
            }
            var result = await TodoUseCase.GetTodo(new PaginationParameter
            {
                PageSize = paginationParameter.PageSize,
                PageNumber = paginationParameter.PageNumber
            });

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(
                new
                {
                    result.TotalCount,
                    result.TotalPages,
                    result.PageSize,
                    result.CurrentPage,
                    result.HasNext,
                    result.HasPrevious
                }));

            return Ok(result);
        }

        /// <summary>
        /// Get todo by identifier
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns>todo</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDoInListDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("id", $"id should be greater than 0 {id}");
                return BadRequest(ModelState);
            }
            var result = await TodoUseCase.GetTodo(id);
            if (result == null)
            {
                return NotFound($"ToDo with id = {id} does not exist");
            }
            return Ok(new ToDoInListDto
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                Status = result.Status,
                ImageUrl = result.ImageUrl,
                Tasks = result.Tasks.Select(t => new TodoItemInListDto()).ToList()
            });
        }

        /// <summary>
        /// Create new todo
        /// </summary>
        /// <param name="dto">todo to create</param>
        /// <returns>Status Code</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> AddToDo([FromBody] ToDoForCreationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await TodoUseCase.AddToDo(new ToDoForCreationMessage(dto.Title, dto.Description, dto.ImageUrl));

            return Ok();
        }

        /// <summary>
        /// Update todo
        /// </summary>
        /// <param name="input">todo to update</param>
        /// <returns>Status Code</returns>
        [HttpPut]
        public async Task<IActionResult> Update(ToDoForUpdateDto input)
        {
            if (input?.Id <= 0 ||
                string.IsNullOrEmpty(input?.Title) && string.IsNullOrEmpty(input?.Description) && string.IsNullOrEmpty(input?.ImageUrl))
            {
                ModelState.AddModelError("id", $"Id should be greater 0 and all fields cannot be null or empty  {input?.Id}");
                return BadRequest(ModelState);
            }

            await TodoUseCase.UpdateToDo(new ToDoForUpdateMessage(input.Id, input.Title, input.Description, input.ImageUrl));

            return Ok();
        }

        /// <summary>
        /// Delete Todo
        /// </summary>
        /// <param name="id">The Identifier</param>
        /// <returns>Status Code</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("id", $"id should be greater than 0 {id}");
                return BadRequest(ModelState);
            }

            await TodoUseCase.DeleteToDo(id);

            return Ok();
        }
    }
}